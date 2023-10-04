# KnitterNotebookProject

### WPF application for knitters. It helps you track progress of your work and make notes about project you work at.
The application is available in two languages (Polish and English). It uses user's computer's default settings to set the language.

* The application allowes you to create a sample of project before you start, for example if you want to knit up warm Christmas sweater.
* There are 3 statuses of project - Planned project, Project in progress and Finished project.
* You start at planned project, you can attach .pdf file with pattern to knit up. When you move it to planned projects you can add photos of progress. When you finish knitting up your clothes you can move it to finished projects.
* You can edit your project if it is planned or in progress, for example if you want to change yarns, needles or start date.
* You can always change project's status to previous one, for example move it from finished projects to planned projects if you want to keep knitting up and add more photos of your art (you won't loose any data and photos despite changing project's status). 

### Main technologies and packages:
* .NET 7. and C# 11
* MS SQL Server and Entity Framework Core 7 (local database)
* xUnit
* Fluent Assertions
* Serilog
* BCrypt.Net-Next
* Fluent Validation
* Community.Toolkit.Mvvm
* Gu.Wpf.Adorners
* MailKit
* MaterialDesignThemes

### Presentation
I use English version of application to present it.

Login window allowes you to sign in, you can also create new account or reset your password.
![1](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/4241b4ae-a631-4d36-9fac-f0d042e3babb)

You need e-mail, nickname and password to create new account.

![2](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/8db20541-eaae-4e48-a6a8-c814213eec4c)

If you want to reset your password you will get token valid for 1 day to verify your account and you can set new password.
Important: it is open-source project for learning purposes so I didn't send email account credentials to remote repository due to safety so you can't restore password if you clone this repository.

![3](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/1ecc918e-4d37-43cb-b39b-460568f79338)

![4](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/d3c5bb48-cdaa-48d0-97a8-a99b876c5099)

Knitters usually create samples before they start working at main project. You can add samples with attached photo of yarn of expected sample.

![5](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/f7967cd5-238e-434f-9d13-104a17658576)

When you have many samples you can easily filter their by needle's size. You can delete sample using context menu.

![6](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/5484c36e-cc12-422d-8152-0b41c369f891)

You can add links to tutorials and other helping resources. It opens in default web browser after mouse double click. You can delete the link using context menu.

![7](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/3bb6f375-019e-40d2-9fb9-4274d7d29e30)

There are 3 statuses of project - planned project, project in progress and finished project.

If you want to knit up something you should plan out a project. Choose "Planned projects" button and add new plan to your resources.
You can specify all necessary details and if you want you can attach .pdf file with pattern.

![8](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/7a800b38-6313-4c1d-8581-1b2f54da1974)

You can filter your planned projects by project's name.
You can take more actions using context menu.

![9](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/2506e7ae-03cf-4bbe-8e49-0fb63499db6a)

If your project's status is "planned" or "in progress" you can edit it very easily by choosing this option from context menu.

![10](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/14c39997-db7c-4bf5-9c4a-2d97231f70e8)

When you choose "Start Project" from context menu it will be moved to projects in progress.
The main purpose of projects in progress is to add photos of your progress. 

![11](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/304690a3-35c2-4835-8d50-412aeac752bc)

![12](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/62440784-5931-4203-81dc-6db85f503150)

Despite looking at pattern in "Projects in progress" or "Finished projects" tab, you can also show the pattern in separate window.

![17](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/fe336a28-d988-48e8-aea1-3f1e2b83cae8)

As you can see many images are added. When you compare this photo with previous photo you can see progress of knitting up a sweater.
You can take more actions using context menu for example edit your project or delete it. You can also move your project in progress to planned projects or finish your project.

![13](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/ed4df59d-9f24-4191-b8df-ace8ecc038a7)

If you finish your project end date is set automatically. You can see at the picture that the whole sweater was knitted up.
There are shown all project's details, photos and .pdf file with pattern.

![14](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/5f402bc3-86cf-4822-9466-1970364eef4e)

You can take more actions using context menu. As you can see I moved finished project to project in progress and then again to finished projects so end date changed automatically.

![21](https://github.com/JakubCzura/KnitterNotebookProject/assets/76125047/43659dd4-7f34-4ac7-8247-a03ef68c5a43)


### Thanks
I would like to thank my girlfriend Karolina for giving me idea to create this project. She told me all her expectations what the application should do.
Thanks for my colleagues from AMC Tech who gave me good advices about programming in C#. 

### Contact
If you see a bug, piece of code that should be refactored or want to talk about C# you can always contact me:
* LinkedIn: www.linkedin.com/in/jakub-czura-4b3891253
* GitHub: https://github.com/JakubCzura
