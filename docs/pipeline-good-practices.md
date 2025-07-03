
# Azure DevOps Pipelines: Good Practices

Azure DevOps Pipelines are well documented in the [official documentation](https://learn.microsoft.com/en-us/azure/devops/pipelines/?view=azure-devops).

Besides the official documentation, this document provides some good practices for Azure DevOps Pipelines.

## Getting started

- Unterstand the [key concepts of Azure Pipelines](https://learn.microsoft.com/en-us/azure/devops/pipelines/get-started/key-pipelines-concepts?view=azure-devops).

## Well structured file

One main thought in structuring your Azure DevOps YAML files is to apply Clean Code and SOLID principles to your YAML files. 

This means that you should separate concerns and responsibilities in your YAML files.

This can be achieved by using parameters, variables, resources, and templates.

### Example: azure-pipelines.yml

```yaml
# Example: azure-pipelines.yml

trigger:
  include: 
pr:
  branches:
    include:
      - main

pool:

parameters:
  - name: parameter
    value: value      


variables: # Can be derived from parameters
  - group: Library in Azure DevOps
  - name: some
    value: some-value
  - name: derived-from-parameter
    value: some${{ parameters.parameter }}
  - ${{ if eq(variables['Build.SourceBranch'], 'refs/heads/main') }}:
      - name: environment
        value: Production
  - ${{ elseif eq(variables['Build.Reason'], 'PullRequest') }}:
      - name: environment
        value: Consolidation
  - ${{ else }}:
      - name: environment
        value: Integration
       

resources: # Binding of template repositories
  repositories:
    - repository: sharedTemplates
      type: git
      name: Central.PipelineTemplates
      ref: refs/tags/v1.1.2

# now only templates
stages:
  - template: path/to/template
    parameters:
      debug: $(some) # This doesn't work with dynamically created variables in a step   
```  

- Separate stages for separate concerns
- Use separated stages for purposes like Test, Build, Publishing

## Templates

### Naming

`use-kebab-case` for file names and folder names, e.g. `build-docker-image.yml`, `test-application.yml`, `publish-release.yml`.

### Folder structure

The folder structure should be organized in a way that reflects the purpose of the templates. A common structure is:

```text
- 🚀 azure-pipelines.yml
- 📁 .azure-pipelines or template purpose e. g. test/build/ 
  - 📁 variables
  - 📁 stages
    - contains stage templates
    - should only contain very small jobs or referencing job templates
  - 📁 jobs
    - contains job templates
    - should only contain very small amount of steps
    - or better: step templates
  - 📁 steps
    - contains step templates
    - should not be too big and precise in the naming
```

### VSC extension

For editing Azure DevOps YAML files in Visual Studio Code, I recommend the following setup:

- Use the [Azure Pipelines extension for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-azure-devops.azure-pipelines) to get syntax highlighting and validation.
- For this I recommend to organize your git repositories locally by your Azure DevOps organization, e.g. `C:\git\my-organization\my-project\`
- Create in the organization folder a file called `ado-pipelines-schema.json` with the content of the schema from your Azure DevOps organization, e.g. `https://dev.azure.com/ohlrogge/_apis/distributedtask/yamlschema`.
-  Edit your workspace's settings.json (Preferences Open User Settings) to include this:

```json
"azure-pipelines.customSchemaFile": "../ado-pipelines-schema.json",
"[azure-pipelines]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
},
```

### Automated versioning

- Use [GitVersion](https://github.com/GitTools/actions/blob/main/docs/examples/azure/gitversion/index.md) to automatically generate semantic versioning based on your git history and commit messages.
- Gitversion template is based on [semvver](https://semver.org/) 
- Write your commit messages according to [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/).
- Configure your pipeline to use the GitVersion task to set the version variables.
