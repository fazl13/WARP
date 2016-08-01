﻿using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;

namespace WARP
{
    public class AppPage
    {
        // Текст на вкладке браузера
        public string BrowserTabTitle { get; set; } = string.Empty;

        // Ширина карточки
        public int EditDialogWidth { get; set; } = 0;

        // Грид шапки
        public TableData Master { get; set; } = null;

        // Список подчиненных шапке таблиц
        public List<TableData> Detail { get; set; } = null;

        // Скрипт бинда изменения размеров рабочей области
        public virtual string GenerateJSWindowsResize()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("        $(window).bind('resize', function () {");
            sb.AppendLine("            $('.dataTables_scrollBody').css('height', ($(window).height() - 125) + 'px');");
            sb.AppendLine("        });");
            return sb.ToString();
        }

        public virtual string MouseDoubleClick()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("            $('#table" + Master.TableSql + "').on('dblclick', 'tr', function() {");
            sb.AppendLine("                var table = $('#table" + Master.TableSql + "').DataTable();");
            sb.AppendLine("                var id = table.row(this).id();");
            sb.AppendLine("                $('#EditDialog').modal();");
            // sb.AppendLine("                $('#EditDialogContent').html('Загрузка..');");
            sb.AppendLine("                $('#EditDialogContent').load('/Handler/EditDialogHandler.ashx?curBase=" + Master.SqlBase + "&curTable=" + Master.TableSql + "&curPage=" + Master.PageName + "&action=edit&curId=' + id);");
            sb.AppendLine("            });");
            return sb.ToString();
        }

        public virtual string ContextMenu()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("            $.contextMenu({");
            sb.AppendLine("                selector: '#table" + Master.TableSql + " .selected td',");
            sb.AppendLine("                items: {");
            sb.AppendLine("                    foo: {");
            sb.AppendLine("                        name: 'Foo',");
            sb.AppendLine("                        callback: function(key, opt) {");
            sb.AppendLine("                            alert('Foo!');");
            sb.AppendLine("                        }");
            sb.AppendLine("                    },");
            sb.AppendLine("                    bar: {");
            sb.AppendLine("                        name: 'Bar',");
            sb.AppendLine("                        callback: function(key, opt) {");
            sb.AppendLine("                            alert('Bar!')");
            sb.AppendLine("                        }");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            });");

            return sb.ToString();
        }

        public virtual string GenerateEditDialog(string curId)
        {
            DataRow data = null;
            switch (Master.Action)
            {
                case TableAction.Create:
                    break;

                case TableAction.Edit:
                    DataTable dt = Master.GetData(curId);
                    if (dt.Rows.Count == 0)
                    {
                        return "Not found id = " + curId;
                    }
                    else
                    {
                        data = dt.Rows[0];
                    }
                    break;

                case TableAction.Remove:
                    break;

                default:
                    break;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<div class=\"card-modal-header\">");
            sb.AppendLine("     <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>");
            sb.AppendLine("     <div id=\"HeaderMsg\" class=\"label label-success card-modal-header-msg\"></div>");
            sb.AppendLine("     <div id=\"HeaderError\" class=\"label label-danger card-modal-header-msg\"></div>");
            switch (Master.Action)
            {
                case TableAction.Create:
                    sb.AppendLine("     <h4 class=\"modal-title\">Новая запись</h4>");
                    break;

                case TableAction.Edit:
                case TableAction.Remove:
                    sb.AppendLine("     <h4 class=\"modal-title\">Запись № " + curId.ToString() + "</h4>");
                    sb.AppendLine("     <h6 class=\"modal-title\">Дата редактирования: " + data["DateUpd"].ToString() + " " + data["User"].ToString() + "</h6>");
                    break;

                default:
                    break;
            }
            sb.AppendLine("</div>");
            sb.AppendLine("<div id=\"EditDialogBody\" class=\"modal-body\">");
            sb.AppendLine("     <div class=\"row\">");
            sb.AppendLine("     <form method=\"POST\" id=\"EditForm\" action=\"javascript: void(null);\" onsubmit=\"submit()\">");

            // Inputs
            string value = string.Empty;
            string valueText = string.Empty;
            StringBuilder js = new StringBuilder();
            foreach (TableColumn tableColumn in Master.ColumnList)
            {
                // Текущие или стандарные значения для инпута
                if (tableColumn.EditType != TableColumnEditType.None)
                    switch (Master.Action)
                    {
                        case TableAction.Create:
                            value = tableColumn.EditDefaultValue;
                            valueText = tableColumn.EditDefaultText;///!!!
                            break;

                        case TableAction.Edit:
                            value = data[tableColumn.DataNameSql].ToString();
                            if (tableColumn.EditType == TableColumnEditType.Autocomplete || tableColumn.EditType == TableColumnEditType.DropDown)
                                valueText = (data[tableColumn.DataLookUpResult] ?? string.Empty).ToString();
                            else
                                valueText = string.Empty;
                            break;
                    }

                // Выбираем тип, генерим инпут
                switch (tableColumn.EditType)
                {
                    case TableColumnEditType.Date:
                        sb.AppendLine("             <div class=\"card-input-group\">");
                        sb.AppendLine("                 <label class=\"card-label\" >" + tableColumn.ViewCaption + "</label>");
                        sb.AppendLine("                     <input id=\"" + tableColumn.DataNameSql + "\" name=\"" + tableColumn.DataNameSql + "\" class=\"card-form-control\" value=\"" + value + "\" >");
                        sb.AppendLine("                 <div id=\"" + tableColumn.DataNameSql + "Error\" class=\"card-input-error\">&nbsp;</div>");
                        sb.AppendLine("             </div>");
                        js.AppendLine("$('#" + tableColumn.DataNameSql + "').mask('99.99.9999',{ placeholder: 'дд.мм.гггг'}); ");
                        js.AppendLine("$('#" + tableColumn.DataNameSql + "').datetimepicker({locale: 'ru', useCurrent:false, format: 'DD.MM.YYYY',}); ");
                        break;

                    case TableColumnEditType.String:
                        sb.AppendLine("             <div class=\"card-input-group\">");
                        sb.AppendLine("                 <label class=\"card-label\" >" + tableColumn.ViewCaption + "</label>");
                        sb.AppendLine("                 <input id=\"" + tableColumn.DataNameSql + "\" name=\"" + tableColumn.DataNameSql + "\" class=\"card-form-control\" value=\"" + value + "\" >");
                        sb.AppendLine("                 <div id=\"" + tableColumn.DataNameSql + "Error\" class=\"card-input-error\">&nbsp;</div>");
                        sb.AppendLine("             </div>");
                        break;

                    case TableColumnEditType.DropDown:
                    case TableColumnEditType.Autocomplete:
                        sb.AppendLine("             <div id=\"scrollable-dropdown-menu\">");
                        sb.AppendLine("                 <div class=\"card-input-group\">");
                        sb.AppendLine("                     <label class=\"card-label\" >" + tableColumn.ViewCaption + "</label>");
                        sb.AppendLine("                     <input type=\"text\"  id=\"" + tableColumn.DataLookUpResult + "\" onchange=\"if ($('#" + tableColumn.DataLookUpResult + "').val().trim() == '')$('#" + tableColumn.DataNameSql + "').val(0);\" ");
                        sb.AppendLine("                         class=\"card-form-control\"  value=\"" + valueText + "\" placeholder=\"Начните вводить для поиска по справочнику..\">");
                        sb.AppendLine("                     <input type=\"hidden\" id=\"" + tableColumn.DataNameSql + "\" name=\"" + tableColumn.DataNameSql + "\" value=\"" + value + "\">");
                        sb.AppendLine("                     <div id=\"" + tableColumn.DataNameSql + "Error\" class=\"card-input-error\">&nbsp;</div>");
                        sb.AppendLine("                 </div>");
                        sb.AppendLine("             </div>");

                        js.AppendLine();
                        js.AppendLine("            // Для столбца: " + tableColumn.ViewCaption);
                        js.AppendLine("            var source" + tableColumn.DataLookUpResult + " = new Bloodhound({");
                        js.AppendLine("                datumTokenizer: Bloodhound.tokenizers.whitespace,");
                        js.AppendLine("                queryTokenizer: Bloodhound.tokenizers.whitespace,");
                        js.AppendLine("                remote: {");
                        js.AppendLine("                    url: '/Handler/TypeaheadHandler.ashx?t=" + tableColumn.DataLookUpTable + "&q=%QUERY',");
                        js.AppendLine("                    wildcard: '%QUERY'");
                        js.AppendLine("                },");
                        js.AppendLine("                limit: 30,");
                        js.AppendLine("            });");
                        js.AppendLine();
                        js.AppendLine("            $('#scrollable-dropdown-menu #" + tableColumn.DataLookUpResult + "').typeahead({");
                        js.AppendLine("                highlight: true,");
                        js.AppendLine("                minLength: " + (tableColumn.FilterType == TableColumnFilterType.DropDown ? "0" : "1") + ",");
                        js.AppendLine("            },");
                        js.AppendLine("            {");
                        js.AppendLine("                name: 'th" + tableColumn.DataLookUpResult + "',");
                        js.AppendLine("                display: 'Name',");
                        js.AppendLine("                highlight: true,");
                        js.AppendLine("                limit: 30,");
                        js.AppendLine("                source: source" + tableColumn.DataLookUpResult + ",");
                        js.AppendLine("            });");
                        js.AppendLine();
                        js.AppendLine("            $(\"#" + tableColumn.DataLookUpResult + "\").on(\"typeahead:selected typeahead:autocompleted\", function (e, datum) { $(\"#" + tableColumn.DataNameSql + "\").val(datum.ID); });");
                        break;

                    case TableColumnEditType.Integer:
                        sb.AppendLine("             <div class=\"card-input-group\">");
                        sb.AppendLine("                 <label class=\"card-label\" >" + tableColumn.ViewCaption + "</label>");
                        sb.AppendLine("                     <input id=\"" + tableColumn.DataNameSql + "\" name=\"" + tableColumn.DataNameSql + "\" class=\"card-form-control\" value=\"" + value + "\" >");
                        sb.AppendLine("                 <div id=\"" + tableColumn.DataNameSql + "Error\" class=\"card-input-error\">&nbsp;</div>");
                        sb.AppendLine("             </div>");
                        break;

                    case TableColumnEditType.Money:
                        sb.AppendLine("             <div class=\"card-input-group\">");
                        sb.AppendLine("                 <label class=\"card-label\" >" + tableColumn.ViewCaption + "</label>");
                        sb.AppendLine("                 <input id=\"" + tableColumn.DataNameSql + "\" name=\"" + tableColumn.DataNameSql + "\" class=\"card-form-control\" value=\"" + value + "\" >");
                        sb.AppendLine("                 <div id=\"" + tableColumn.DataNameSql + "Error\" class=\"card-input-error\">&nbsp;</div>");
                        sb.AppendLine("             </div>");
                        js.AppendLine();
                        js.AppendLine("             $('#" + tableColumn.DataNameSql + "').val(accounting.formatNumber($('#" + tableColumn.DataNameSql + "').val().trim().replace(',', '.'), 2, ' '));");
                        js.AppendLine("             $('#" + tableColumn.DataNameSql + "').bind('blur', function(event) {");
                        js.AppendLine("                 this.value = accounting.formatNumber(this.value.trim().replace(',', '.'), 2, ' ');");
                        js.AppendLine("             });");
                        js.AppendLine("             $('#" + tableColumn.DataNameSql + "').bind('focus', function(event) {");
                        js.AppendLine("                 this.value = accounting.unformat(this.value.trim());");
                        js.AppendLine("             });");
                        break;
                    default:
                        break;
                }
            }

            sb.AppendLine("     </form>");// row
            sb.AppendLine("     </div>");// row
            sb.AppendLine("     <div class=\"row\">");
            sb.AppendLine("     таб часть будет тут");
            sb.AppendLine("     </div>");
            sb.AppendLine("     <div class=\"row\">");
            sb.AppendLine("     </div>");

            sb.AppendLine("<div class=\"card-modal-footer\">");
            sb.AppendLine("     <div class=\"card-modal-footer-left\">");
            sb.AppendLine("         <button type=\"button\" class=\"btn btn-default btn-sm\">New</button>");
            sb.AppendLine("     </div>");
            sb.AppendLine("     <div class=\"card-modal-footer-rigth\">");
            sb.AppendLine("         <button type=\"button\" class=\"btn btn-default btn-sm\" data-dismiss=\"modal\">Close</button>");
            sb.AppendLine("         <button type=\"button\" id =\"SaveButton\" class=\"btn btn-primary btn-sm\" onclick=\"submit()\" disabled>Save changes</button>");
            sb.AppendLine("     </div>");
            sb.AppendLine("</div>");
            sb.AppendLine();
            sb.AppendLine("<script>");
            sb.AppendLine();
            sb.AppendLine("     $('#EditDialog input').bind('change keyup', function(event) {AllowSave();});"); // Активирует кнопку Сохранить при изменениях в инпутах
            sb.AppendLine(js.ToString());
            sb.AppendLine();
            sb.AppendLine("     function AllowSave() {"); // Снимает блок с кнопки «Сохранить»
            sb.AppendLine("         $('#SaveButton').prop('disabled', false);");
            sb.AppendLine("     }");
            sb.AppendLine();
            sb.AppendLine("     function submit() {");
            sb.AppendLine("         var msg = $('#EditForm').serialize();");
            sb.AppendLine("         $.ajax({");
            sb.AppendLine("             type: 'POST',");
            sb.AppendLine("             url: '/Handler/CardSaveDataHandler.ashx?curBase=" + Master.SqlBase + "&curTable=" + Master.TableSql + "&curPage=" + Master.PageName + "&curId=" + curId + "&action=" + Master.Action + "', ");
            sb.AppendLine("             data: msg,");
            sb.AppendLine("             success: function(data) {");
            sb.AppendLine("                $('.card-input-error').html('&nbsp;');");// убираем предыдущие сообщения об ошибках
            sb.AppendLine("                $('.card-input-group').removeClass('has-error');"); // убираем красный цвет
            sb.AppendLine("                if (data.indexOf('fieldErrors')!=-1){");
            sb.AppendLine("                     var jdata = JSON.parse(data);");
            sb.AppendLine("                     jdata.fieldErrors.forEach(function(item, i, arr) {");
            sb.AppendLine("                         $('#'+item.name+'Error').html(item.status);");
            sb.AppendLine("                         $('#'+item.name).parent().addClass('has-error');");
            sb.AppendLine("                     });");
            sb.AppendLine("                }");
            sb.AppendLine("                else if (data.indexOf('error')!=-1){");
            sb.AppendLine("                     var jdata = JSON.parse(data);");
            sb.AppendLine("                     $('#HeaderError').hide();");
            sb.AppendLine("                     $('#HeaderError').html(jdata.error);");
            sb.AppendLine("                     $('#HeaderError').fadeIn();");
            sb.AppendLine("                }");
            sb.AppendLine("                else{");
            sb.AppendLine("                     $('#EditDialogContent').load(");
            sb.AppendLine("                         '/Handler/EditDialogHandler.ashx?curBase=" + Master.SqlBase + "&curTable=" + Master.TableSql + "&curPage=" + Master.PageName + "&action=edit&curId=" + (curId != "0" ? curId + "'" : "' + data") + ",null,");
            sb.AppendLine("                         function(){");
            sb.AppendLine("                             $('#HeaderMsg').hide();");
            sb.AppendLine("                             $('#HeaderMsg').html('" + (curId != "0" ? "Запись сохранена" : "Запись создана") + "');");
            sb.AppendLine("                             $('#HeaderMsg').fadeIn();");
            sb.AppendLine("                             setTimeout('$(\"#HeaderMsg\").fadeOut();', 3000);");
            sb.AppendLine("                         }");
            sb.AppendLine("                     );");
            sb.AppendLine("                     ");
            sb.AppendLine("                }");
            sb.AppendLine("             },");
            sb.AppendLine("             error: function(xhr, str){alert('Возникла ошибка: ' + xhr.responseCode);}");
            sb.AppendLine("         }); ");
            sb.AppendLine("     } ");
            sb.AppendLine("</script>");
            return sb.ToString();
        }

        // HTML| JS общие для всей страницы
        public string GenerateContent()
        {
            StringBuilder sb = new StringBuilder();

            // Таблица HTML для шапки
            sb.AppendLine(Master.GenerateHtmlTable());

            // Карточка
            sb.AppendLine("    <div id=\"EditDialog\" class=\"modal\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"myLargeModalLabel\">");
            sb.AppendLine("         <div class=\"modal-dialog modal-lg\" " + (EditDialogWidth > 0 ? "style=\"width: " + EditDialogWidth + "px;\"" : "") + " role=\"document\">");
            sb.AppendLine("                 <div id=\"EditDialogContent\" class=\"modal-content\">");
            sb.AppendLine("                 </div>");
            sb.AppendLine("         </div>");
            sb.AppendLine("    </div>");

            // Фильтр HTML для шапки
            sb.AppendLine(Master.GenerateFilterFormDialog());

            // Блок JS
            sb.AppendLine("    <script>");
            sb.AppendLine("        var editor;"); // Глобальная переменная для editor'a
            sb.AppendLine(GenerateJSWindowsResize());
            sb.AppendLine("        $(document).ready(function () {");
            sb.AppendLine("            document.title = '" + BrowserTabTitle + "';");
            sb.AppendLine("            $('#curPageTitle').text('" + Master.PageTitle + "');");
            sb.AppendLine(Master.GenerateJSEditorInit());
            sb.AppendLine(Master.GenerateJSDataTable());
            sb.AppendLine(MouseDoubleClick());
            sb.AppendLine(ContextMenu());
            sb.AppendLine("            $(window).resize();");

            sb.AppendLine("        });");
            sb.AppendLine("    </script>");

            return sb.ToString();
        }

        // Обрабатывает запросы на редактирование данных
        public string Process()
        {
            // Ответ
            string result = string.Empty;

            // AJAX|JSON
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            //------
            // Шапка
            //------

            // Чекаем на простые условия (обязательность, длинна и вообще изменилось ли что нибудь)
            if (Master.Action != TableAction.Remove)
                result = Master.Validate();

            // Чекаем на условия конкретной таблицы
            if (string.IsNullOrEmpty(result))
                result = Master.Check();

            //------
            // Курсоры
            //------
            if (Detail != null && string.IsNullOrEmpty(result))
                foreach (TableData tableData in Detail)
                {
                    // Чекаем на простые условия (обязательность, длинна и вообще изменилось ли что нибудь)
                    if (string.IsNullOrEmpty(result) && tableData.Action != TableAction.Remove)
                        result = tableData.Validate();

                    // Чекаем на условия конкретной таблицы
                    if (string.IsNullOrEmpty(result))
                        result = tableData.Check();
                }

            // Сохраняем
            if (string.IsNullOrEmpty(result))
                result = Master.Save();

            return result;
        }
    }
}