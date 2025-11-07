# Azure DevOps Training: Tasks for Azure Pipelines

This repository contains tasks to learn how to use Azure Pipelines in Azure DevOps. The tasks are designed to be completed in order, starting with a simple pipeline and gradually introducing more advanced features.

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

> Learning goal: Understand how to use parameters, conditions and expressions in Azure Pipelines.

Update the starter pipeline to use the `parameters` feature of Azure Pipelines.

1. Checkout the repository locally.
1. Move the starter pipeline `.azure-pipelines/main.yml`. I generally recommend to use this dedicated folder for all pipeline YAML files. The `.` prefix shows that it's a config folder.

   ```bash
   mkdir .azure-pipelines
   mv azure-pipelines.yml .azure-pipelines/main.yml
   ```

1. Add `boolean` parameter.
1. Add `string` parameter with default value and allowed values.
1. Add `number` parameter with default value.
1. Print the parameters in a task.
1. Use the task `condition` to run a task only if the boolean parameter is `true`.
1. Use the `if` expression to show and run a task only if boolean parameter is `true`.
1. After running the pipeline, view the full YAML file of the pipeline and investigate the parameters in the file.

> Remark: After renaming the pipeline file, you need to update the pipeline settings in Azure DevOps to point to the new file location.

## Task 02: Variables

> Learning goal: Understand how to use variables in Azure Pipelines.

Update the parameters pipeline to use the `variables` and `variable groups` features of Azure Pipelines.

1. Create in the Azure DevOps Library a variable group named `{your-name}-variables` a secret and a non-secret variable.
1. Add the variable group to the pipeline.
1. Add another variable to the pipeline.
1. Define a variable that is derived from a parameter using an if expression.
1. Append to the parameter tasks additional tasks to print the variables.
1. After running the pipeline, view the Full YAML file of the pipeline and investigate the variables in the file, compare them with the parameters.

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
   1. Convert the following commands into [dotnet core pipeline tasks](https://learn.microsoft.com/en-us/azure/devops/pipelines/tasks/reference/dotnet-core-cli-v2?view=azure-pipelines):

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

1. Instead of using the steps template from your repository, use the steps template from the template repository.
1. Run the pipeline and verify that it works as expected.
1. Revert the changes to use your own steps template

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
1. Create a job that wraps the central test steps template. The job should test the application in parallel to the build.
1. Wrap your job templates in stages, one for the build and test and one for the deployment. The build and test jobs should run in parallel. It's not necessary to create stage templates.
1. Run the pipeline and verify that the build and test jobs run in parallel and the deployment job runs after the build and test jobs have completed successfully.

## Task 08: Implement approvals and checks

> Learning goal: Understand how to use approvals and checks in Azure Pipelines.

Change the deploy job to use the [environment feature](https://learn.microsoft.com/en-us/azure/devops/pipelines/process/environments?view=azure-devops#target-an-environment-from-a-deployment-job) of Azure Pipelines and add an approval before deployment.

1. Create an environment in Azure DevOps name it `{web-app-name}-environment`.
1. Add an approval check to the environment that requires your user to approve the deployment.
1. Change the deploy job to target the environment.
1. Run the pipeline and verify that the deployment job waits for your approval before deploying the application.

> Remark: The deployment job type automatically downloads the artifact, so you can remove the download artifact task from the deploy job.

## Task 09: Implement PR trigger

> Learning goal: Understand how to use PR triggers in Azure Pipelines.

Update the pipeline to trigger on pull requests.

1. Add a new trigger for pull requests in the pipeline YAML file.
1. Create a new branch and make a change to the pipeline file.
1. Specify the branch `main` to be included for the PR trigger.
1. Set a condition on the deploy stage to run only on the `main` branch.
1. Protect the `main` branch in the repository settings and require the pipeline to pass before merging.
1. Create a pull request and verify that the pipeline is triggered.

## Task 10: Automated versioning and tagging

> Learning goal: Understand how to implement automated versioning and tagging in Azure Pipelines.

Update the pipeline to automatically version and tag the commit on successful deployment.

1. Inspect the versioning and tagging implementation in the training repository.
1. Include the versioning and tagging templates for builds running on the `main` branch.
1. The tagging stage should depend on the `Deploy` stage and run only if the deployment was successful.

> Remark: Read the [documentation for consuming variables from other stages](https://learn.microsoft.com/en-us/azure/devops/pipelines/process/variables?view=azure-devops&tabs=yaml%2Cbatch#use-outputs-in-a-different-stage).
