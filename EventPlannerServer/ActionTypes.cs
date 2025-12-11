using Microsoft.AspNetCore.Identity.Data;

namespace EventPlannerServer
{
    public class ActionTypes
    {
        public const string Login = "Вход";
        public const string Register = "Регистрация";
        public const string EventCreated = "Создание события";
        public const string EventDeleted = "Удаление события";
    }
}
