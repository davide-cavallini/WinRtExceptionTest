using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;
using System.Text.Json;

namespace WinRtExceptionTest;

public partial class Page2 : ContentPage
{
    private Grid _grid;

    public Page2()
    {
        InitializeComponent();
        _grid = new Grid()
        {
            Margin = 5,
            ColumnSpacing = 5,
            RowSpacing = 5
        };
    }

    protected async override void OnAppearing()
    {
        await GenerateLayout();
        MainGrid.Children.Clear();
        MainGrid.Add(_grid);
        base.OnAppearing();
    }

    private async Task GenerateLayout()
    {
        var stream = await FileSystem.OpenAppPackageFileAsync("file.json");
        string json = (new StreamReader(stream)).ReadToEnd();
        var data = JsonSerializer.Deserialize<JsonUserInterface>(json);

        if (data is null)
        {
            return;
        }

        var singleSizeAspetRatio = data.CellAspectRatio.Split(':');

        double rowsNumber = data.ColumnsCount /
            (Convert.ToDouble(singleSizeAspetRatio[0]) /
            Convert.ToDouble(singleSizeAspetRatio[1]));

        int delta = 0;
        if (rowsNumber % 1 != 0)
        {
            delta = 1;
        }

        int realRowsNumber = (int)rowsNumber + delta;

        for (int i = 0; i < realRowsNumber; i++)
            _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        for (int i = 0; i < data.ColumnsCount; i++)
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        bool navigateBack = true;
        foreach (var container in data.Containers)
        {
            AddContainer(container, navigateBack);
            navigateBack = false;
        }
    }

    private void AddContainer(Container container, bool navigateBack)
    {
        Border border = new Border
        {
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeShape = new RoundRectangle(),
            Background = Color.FromArgb(container.BackgroundColor)
        };

        if (navigateBack)
        {
            border.Content = new Button()
            {
                Text = "Back",
                Command = new Command(async () =>
                {
                    try
                    {
                        Debug.WriteLine($"{nameof(Page2)} into {nameof(MainPage)}");
                        await Shell.Current.GoToAsync("..");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ExtractErrorMessage(ex));
                    }
                })
            };
        }

        _grid.Add(border, container.StartX, container.StartY);
        _grid.SetColumnSpan(border, container.SizeX);
        _grid.SetRowSpan(border, container.SizeY);
    }

    public string ExtractErrorMessage(Exception ex)
    {
        string errorMessage = ex.Message;
        if (ex.InnerException != null)
        {
            errorMessage += " | " + ex.InnerException.Message;
        }
        if (errorMessage == null || errorMessage == "")
        {
            errorMessage += ex.StackTrace;
        }
        if (errorMessage == null || errorMessage == "")
        {
            errorMessage += "?";
        }
        return errorMessage;
    }
}