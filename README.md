# WPF-MARIO


# ![Screenshot](https://github.com/romankessler/WPF.Mario/blob/master/05-02-_2018_13-07-23.png) 


******

```xml
<Window x:Class="Mario.Eval.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:userControls="clr-namespace:Mario.Eval.UserControls"
        xmlns:people="clr-namespace:Mario.Eval.UserControls.People"
        Title="MainWindow"
        Width="800" Height="800">
        <userControls:HudUserControl>
            <userControls:HudUserControl.AdditionalContent>
            <userControls:MapUserControl MapTerrainImage="{StaticResource ImgBackground}">
                <userControls:MapItemUserControl XPosition="200" YPosition="150" Charset="{StaticResource ImgStein}" ImageType="Image" IsBlocked="True"/>
                <userControls:MapItemUserControl XPosition="300" YPosition="200" Charset="{StaticResource ImgStein}" ImageType="Image" IsBlocked="True"/>
                <userControls:MapItemUserControl XPosition="300" YPosition="400" Charset="{StaticResource ImgStein}" ImageType="Image" IsBlocked="True"/>
                <userControls:MapItemUserControl XPosition="100" YPosition="350" Charset="{StaticResource ImgBusch}" ImageType="Image" IsBlocked="True"/>
                <userControls:MapItemUserControl XPosition="150" YPosition="500" Charset="{StaticResource ImgStein}" ImageType="Image" IsBlocked="True"/>
                <userControls:MapItemUserControl XPosition="450" YPosition="200" Charset="{StaticResource ImgBusch}" ImageType="Image" IsBlocked="True"/>
                <userControls:MapItemUserControl XPosition="50" YPosition="80" Charset="{StaticResource ImgBusch}" ImageType="Image" IsBlocked="True"/>
                <userControls:MapItemUserControl XPosition="350" YPosition="300" Charset="{StaticResource ImgStein}" ImageType="Image" IsBlocked="True"/>
                <people:PlayerUserControl XPosition="150" YPosition="150" Charset="{StaticResource ImgMarioCharset}" KeyMoveLeft="Left" KeyMoveRight="Right" KeyMoveUp="Up" KeyMoveDown="Down"/>
            </userControls:MapUserControl>
        </userControls:HudUserControl.AdditionalContent>
        </userControls:HudUserControl>
</Window>
```
