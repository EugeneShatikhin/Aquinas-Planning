1. Клонировать репозиторий. 
1. Восстановить project.bak на свой SQL Server. 
1. AquinasCore/Views/web.config → поменять connection strings 

![](Setup/readme/Aspose.Words.67b90e2d-d857-4aa2-8d53-64b7005e75de.001.jpeg)

4. AquinasCore/SQL/DbGroupCreator.cs поменять адрес папки тут и в классе DbGroupDeletor в этом же файле ниже. 

![](Setup/readme/Aspose.Words.67b90e2d-d857-4aa2-8d53-64b7005e75de.002.jpeg)

5. Сделать импорт Postman коллекции из DBPRJ.postman\_collection.json 

![](Setup/readme/Aspose.Words.67b90e2d-d857-4aa2-8d53-64b7005e75de.003.png)

6. Запустить сервер. 
6. Зарегистрировать новых юзеров. Их пароль хешируется, поэтому его стоит запомнить.  
6. Во всех запросах в заголовке Authorization нужно прописать username;password пользователя, под которым будут выполняться действия. 

![](Setup/readme/Aspose.Words.67b90e2d-d857-4aa2-8d53-64b7005e75de.004.png)

9. Тестировать! 
