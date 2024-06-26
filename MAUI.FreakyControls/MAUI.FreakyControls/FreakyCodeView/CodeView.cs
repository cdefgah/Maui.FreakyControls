﻿using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Enums;
using Microsoft.Maui.Controls.Shapes;

namespace Maui.FreakyControls;

internal class CodeView : Border
{
    public const double DefaultItemSize = 50.0;
    public const double DefaultDotSize = 20.0;
    public const double DefaultItemSpacing = 5.0;
    public const int DefaultCodeLength = 4;
    public static Color DefaultColor = Colors.Black;
    public static Color DefaultItemBackgroundColor = Colors.Transparent;

    private string inputChar;
    private Color color;
    private Color ItemBorderColor;

    public FocusAnimation FocusAnimationType { get; set; }
    public Color ItemFocusColor { get; set; }
    public Border Item => this;
    public Border Dot { get; }
    public Label CharLabel { get; }

    public CodeView()
    {
        Padding = 0;
        BackgroundColor = DefaultItemBackgroundColor;
        StrokeShape = new Ellipse();
        Stroke = DefaultColor;
        HeightRequest = WidthRequest = DefaultItemSize;
        VerticalOptions = LayoutOptions.Center;

        Dot = new Border()
        {
            StrokeShape = new Ellipse() { Fill = DefaultColor },
            StrokeThickness = 0,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            HeightRequest = DefaultDotSize,
            WidthRequest = DefaultDotSize,
            Scale = 0,
            Padding = 0,
        };

        CharLabel = new Label()
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = DefaultColor,
            FontAttributes = FontAttributes.Bold,
            VerticalTextAlignment = TextAlignment.Center,
            Scale = 0,
            TextTransform = TextTransform.Uppercase
        };

        Content = Dot;
    }

    private async Task GrowAsync()
    {
        await Content.ScaleTo(1.0, 100);
    }

    private async Task ShrinkAsync()
    {
        await Content.ScaleTo(0, 100);
    }

    public void SetColor(Color color, Color ItemBorderColor)
    {
        this.color = color;
        this.ItemBorderColor = ItemBorderColor;
        SetBorderColor();
        Dot.BackgroundColor = color;
        CharLabel.TextColor = color;
    }

    public void SetRadius(ItemShape shapeType)
    {
        if (shapeType == ItemShape.Circle)
        {
            this.StrokeShape = new Ellipse()
            {
                WidthRequest = (float)HeightRequest / 2,
                HeightRequest = (float)HeightRequest / 2
            };
        }
        else if (shapeType == ItemShape.Square)
        {
            this.StrokeShape = new RoundRectangle() { CornerRadius = 0 };
        }
        else if (shapeType == ItemShape.Squircle)
        {
            this.StrokeShape = new RoundRectangle() { CornerRadius = 10 };
        }
    }

    public void SecureMode(bool isPassword)
    {
        if (isPassword)
        {
            Content = Dot;
        }
        else
        {
            Content = CharLabel;
        }

        if (!string.IsNullOrEmpty(inputChar))
        {
            GrowAsync().RunConcurrently();
        }
        else
        {
            ShrinkAsync().RunConcurrently();
        }
    }

    public void ClearValueWithAnimation()
    {
        inputChar = null;
        ShrinkAsync().RunConcurrently();
    }

    public void SetValueWithAnimation(char inputChar)
    {
        UnfocusAnimate();
        CharLabel.Text = inputChar.ToString();
        this.inputChar = inputChar.ToString();
        GrowAsync().RunConcurrently();
    }

    public async void FocusAnimate()
    {
        Stroke = ItemFocusColor;

        if (FocusAnimationType == FocusAnimation.Bounce)
        {
            await this.ScaleTo(1.2, 100);
            await this.ScaleTo(1, 100);
        }
        else if (FocusAnimationType == FocusAnimation.Scale)
        {
            await this.ScaleTo(1.2, 100);
        }
    }

    public async void UnfocusAnimate()
    {
        SetBorderColor();
        await this.ScaleTo(1, 100);
    }

    private void SetBorderColor()
    {
        Stroke = ItemBorderColor == Colors.Black ? color : ItemBorderColor;
    }
}