﻿using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace WARP
{
    /// <summary>
    /// Принимает запросы от карточки (создание/редактирование/удаление)
    /// </summary>
    public class CardSaveDataHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            // База
            string curBase = context.Request["curBase"];

            // Таблица
            string curTable = context.Request["curTable"];

            // Страница
            string curPage = context.Request["curPage"];

            // Действие
            Action tableAction = Func.ParseEnum<Action>(context.Request["action"]);

            // Id
            string curId = context.Request["curId"];


            // Список переданных строк, ключ - ID
            Dictionary<string, List<RequestData>> requestRows = new Dictionary<string, List<RequestData>>();
            requestRows.Add(curId, new List<RequestData>());

            // Парсим переданные параметры
            foreach (string item in context.Request.Form.AllKeys)
            {
                requestRows[curId].Add(new RequestData { FieldName = item, FieldValue = context.Request.Form[item].Trim() });
            }

            // Инитим соотвествующий класс
            switch (curTable)
            {
                //case "Archive":
                //    AppPage appPage = new AppPageArchive();
                //    appPage.Master.Init(curBase, curTable, curPage);
                //    appPage.Master.Action = tableAction;
                //    appPage.Master.RequestRows = requestRows;
                //    if (context.Request.Files.Count > 0)
                //        appPage.Master.RequestFiles = context.Request.Files;

                //    // Обрабатываем запрос
                //    context.Response.Write(appPage.Process());
                //    break;

                case "Archive":
                    context.Response.Write(Archive.Process(curBase, curTable, curPage, tableAction, requestRows, context.Request.Files));
                    break;

                default:
                    break;
            }
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}