# Azure DevOps Training: Tasks for Azure Pipelines

## Task 00: Starter pipeline

> Learning goal: Understand the basic structure of an Azure DevOps pipeline and how to run scripts.

In your Repository, create a simple starter pipeline.

1. Go to Pipelines in Azure DevOps
1. Click on "New Pipeline"
1. Select "Azure Repos Git" as the source
1. Select your repository
1. Select "Starter pipeline"
1. Review the generated YAML file and click "Run" to create the pipeline

The pipeline will contain two `script` tasks. The `script` runs in the default command shell of the agent, which is `bash` on Linux and `cmd` on Windows.

To run a script in a specific shell, you can use the `bash` or `pwsh` task, depending on the shell you want to use.

## Task 01: Parameters

> Learning goal: Understand how to use parameters in Azure Pipelines.

Update the starter pipeline to use the `parameters` feature of Azure Pipelines.

1. Add `boolean` parameter.
1. Add `string` parameter with default value and allowed values.
1. Add `number` parameter with default value.
1. Print the parameters in a task.
1. Use the task `condition` to run a task only if the boolean parameter is `true`.
1. Use the `if` expression to show and run a task only if boolean parameter is `true`.
1. After running the pipeline, download the Raw YAML file of the pipeline and investigate the parameters in the file.

## Task 02: Variables

> Learning goal: Understand how to use variables in Azure Pipelines.

Update the parameters pipeline to use the `variables` and `variable groups` features of Azure Pipelines.

1. Create in the Azure DevOps Library a variable group named `{your-name}-variables` a secret and a non-secret variable.
1. Add the variable group to the pipeline.
1. Add another variable to the pipeline.
1. Define a variable that is derived from a parameter using an if expression.
1. Append to the parameter tasks additional tasks to print the variables.
1. After running the pipeline, download the Raw YAML file of the pipeline and investigate the variables in the file, compare them with the parameters.

## Task 03: Build your first application

> Learning goal: Understand how to run build tasks in Azure Pipelines.

Update the variables pipeline to build a simple Blazor application.

1. Prepare a sample project in your repository. This requires .NET Core SDK installed on your machine. If you don't have it installed, you can use the [official .NET installation guide](https://dotnet.microsoft.com/en-us/download/dotnet).
1. Create the sample project in your repository using the following commands:
    ```bash
    # Create src folder
    mkdir src
    # Create a new solution
    cd src && dotnet new sln -n Training
    # Create a folder for the sample project
    mkdir BlazorApp
    # Create a new blazor app
    cd BlazorApp && dotnet new blazor
    # Add the project to the solution
    cd .. && dotnet sln add BlazorApp/BlazorApp.csproj
    ```
1. Clear the starter pipeline and add variables for the `solutionFolder` using the predefined variable `Build.SourcesDirectory`, the `solutionPattern` and an `artifactName`.
1. Add tasks to the pipeline:
   1. Set up the .NET Core SDK version
   1. Convert the following commands into pipeline tasks:
      ```bash
      # Restore dependencies
      dotnet restore
      # Build the project
      dotnet build --configuration Release --no-restore
      # Create the artifact
        dotnet publish --configuration Release --no-build --output $TEMP
      ```
   1. Publish the artifact as Pipeline artifact with the name defined in the variable `artifactName`.
1. Run the pipeline and verify that the artifact can be downloaded and contains the published Blazor app.

## Task 04: Use templates

> Learning goal: Understand how to use templates in Azure Pipelines.

Update the build pipeline to use templates.

1. Create a steps template following the folder structure in the [good practices](docs/pipeline-good-practices.md#folder-structure).
1. Move the steps from the previous task into the template and identify the parameters that are required to run the steps.
1. Use the template in the pipeline and pass the required parameters.
1. Run the pipeline and verify that it works as expected.

## Task 05: Use resources

> Learning goal: Understand how to use another repository as a resource in Azure Pipelines.

Update the template pipeline to use another repository as a resource.

1. Instead of using the steps template from your repository, use the steps template from the training repository.
1. Run the pipeline and verify that it works as expected.
1. Revert the changes to use your original steps template

## Task 06: Use jobs and service connections

> Learning goal: Understand how to use jobs and service connections in Azure Pipelines.

Create a jobs template that wraps the steps template from the previous task and use it in the pipeline. Additionally add a job to deploy the artifact to Azure.

1. Create another steps template that uses the [DownloadPipelineArtifact@2](https://learn.microsoft.com/en-us/azure/devops/pipelines/tasks/reference/download-pipeline-artifact-v2?view=azure-pipelines) and [AzureWebApp@1](https://learn.microsoft.com/en-us/azure/devops/pipelines/tasks/reference/azure-web-app-v1?view=azure-pipelines) tasks.
1. Wrap the steps templates in two separate job templates, one for the build and one for the deployment.
1. Use the job templates in the pipeline and pass the required parameters.
1. Run the pipeline and verify that your Blazor app is reachable via the Azure Web App URL.

## Task 07: Use stages

> Learning goal: Understand how to use stages in Azure Pipelines.

Create a `build and test` stage and a `deploy` stage in the pipeline.

1. Copy the current solution from the training repository to your repository.
1. Create a job and steps template to test the application in parallel to the build.
1. Wrap your job templates in stages, one for the build and test and one for the deployment. The build and test jobs should run in parallel. It's not necessary to create stage templates.
1. Run the pipeline and verify that the build and test jobs run in parallel and the deployment job runs after the build and test jobs have completed successfully.