# Demo 2: ML.NET Tooling

This demo consumes the Taxi Fare model created in [Demo 1](https://github.com/briacht/MLADS-2021-mlnet-tutorial/tree/main/Demo%201%20-%20Explore%20API%20in%20Notebooks) and then uses ML.NET Model Builder in Visual Studio and AutoML to train a sentiment analysis (classification) model.

![Model Builder](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2021/03/model-builder.png)


## Getting started

### Set up
#### Model Builder:
1. Install [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/).
2. Install any .NET Workload and check the optional ML.NET Model Builder (Preview) component.
![VS Installer Workloads](https://dotnet.microsoft.com/static/images/vs-installer-model-builder.png?v=vmvdf1n9u-IZSU3TCZz8Xb9lmlao4aJo5158Ghyt-f8)
4. Enable the Preview Feature by opening Visual Studio and going to **Tools > Options > Environment > Preview Features** and checking **Enable ML.NET Model Builder**.
![Preview Features Options Dialog](https://dotnet.microsoft.com/static/images/enable-model-builder.png?v=givtGJTkQK9b7pWvl2NvoM9Txun8DH0JEcBKvbQbAZs)

#### ML.NET CLI (cross-platform):
1. Install the latest [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0).
2. Open any command line and run:
```dotnet tool install -g mlnet ```
3. Run the following command to see the available options for ML.NET CLI:
```mlnet -h ```

Note: If you're using a console other than Bash (for example, zsh, which is the new default for macOS), then you'll need to give mlnet executable permissions and include mlnet to the system path. Instructions on how to do this should appear in the terminal when you install mlnet (or any global tool).

### Opening Model Builder

1. Open or create a new project in Visual Studio.
2. In the Solution Explorer, right-click on your project and go to **Add > Machine Learning**.

### Additional resources

- [Model Builder Doc](https://docs.microsoft.com/en-us/dotnet/machine-learning/automate-training-with-model-builder)
- [ML.NET CLI Doc](https://docs.microsoft.com/en-us/dotnet/machine-learning/automate-training-with-cli)
