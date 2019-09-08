![Banner](https://raw.githubusercontent.com/sqrMin1/MonoGame.Forms/master/Logos/Logo_Banner_800.png)

# Welcome to the MonoGame.RuntimeBuilder!

 [![Twitter Follow](https://img.shields.io/twitter/follow/SandboxBlizz.svg?style=flat-square&label=Follow&logo=twitter)](https://twitter.com/SandboxBlizz)
 [![NuGet](https://img.shields.io/badge/NuGet-MonoGame.RuntimeBuilder-blue.svg?style=flat-square&logo=NuGet&colorA=3260c4&colorB=77c433)](https://www.nuget.org/packages/MonoGame.RuntimeBuilder)
  
 The MonoGame.RuntimeBuilder **builds** your **raw** content **asynchronously** to the **.XNB** format **during runtime**. 
 
> This library is a part of the [MonoGame.Forms](https://github.com/sqrMin1/MonoGame.Forms) project, 
but it is fully usable without the MonoGame.Forms library!

 ---
 
### Tutorial

Using the MonoGame.RuntimeBuilder is fairly easy. You could use it for example in a fresh WindowsForms project like this:

```c

// Creating the property.
private RuntimeBuilder _RuntimeBuilder { get; set; }

// Initialize the RuntimeBuilder.
_RuntimeBuilder = new RuntimeBuilder(
                Path.Combine(Application.StartupPath, "working"),           // working directory
                Path.Combine(Application.StartupPath, "working", "obj"),    // intermediate directory
                Path.Combine(Application.StartupPath, "Content"),           // output directory
                TargetPlatform.Windows,                                     // target platform
                GraphicsProfile.Reach,                                      // graphics profile
                true)                                                       // compress the content
            {
                Logger = new StringBuilderLogger()                          // logger
            };
            
// Pick Files & Build Content.
private async void ButtonPickFiles_Click(object sender, System.EventArgs e)
{
    if (openFileDialog.ShowDialog() == DialogResult.OK)
    {
        await _RuntimeBuilder.BuildContent(openFileDialog.FileNames);
    }
}
```

And... that's it!
