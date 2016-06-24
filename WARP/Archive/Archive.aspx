﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Archive.aspx.cs" Inherits="WARP.Archive" %>

<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="server">
    <table id="table_id" class="table table-striped table-bordered table-condensed" style="table-layout: fixed; width: 100%">
        <%--;word-wrap:break-word;--%>
        <thead>
            <tr>
                <%=tableData.GenerateHtmlTableColumns()%>
            </tr>
        </thead>
    </table>

    <%=tableData.GenerateFilterFormDialog()%>

    <%--<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Фильтр</h4>
                </div>
                <div class="modal-body">
                    
                        
                    <div class="row">
                        <div class="col-sm-3">
                            <h5>Контрагент</h5>
                        </div>
                        <div class="col-sm-3">
                            <input type="text" id="FrmContr" class="form-control input-sm" placeholder="Username">
                            <input type="hidden" id="IdFrmContr">
                        </div>
                        <div class="col-sm-6">
                            <input type="text" class="form-control input-sm" placeholder="Username">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Закрыть</button>
                    <button type="button" class="btn btn-primary">Применить</button>
                </div>
            </div>
        </div>
    </div>--%>
    

    <script>
     
        // Для выбора вручную страницы
        //var table = $('#table_id').DataTable(); table.page(4).draw('page');
        $(window).bind('resize', function () {
            var h = $(window).height();
            $('.dataTables_scrollBody').css('height', (h - 125) + 'px');
        });

        var editor;
        $(document).ready(function () {
            document.title = '<%=browserTabTitle%>';
            $('#curPageTitle').text('<%=documentTitle%>');

            editor = new $.fn.dataTable.Editor({
                ajax: "/Handler/GetDataHandler.ashx?curBase=<%=Master.curBaseName%>&curTable=<%=tableData.TableSql%>&curPage=<%=curPage%>",
                table: "#table_id",
                idSrc: 'ID',
                fields: [
                    {
                        label: "NumDoc:",
                        name: "NumDoc",
                    },
                    {
                        label: "Prim:",
                        name: "Prim"
                    },
                ]
            });

            $('#table_id').DataTable({
                dom: '<"row top-toolbar"<"col-sm-4"B><"col-sm-4"p><"col-sm-4"i>>Zrt',
                processing: true,
                serverSide: true,
                ajax: "/Handler/GetDataHandler.ashx?curBase=<%=Master.curBaseName%>&curTable=<%=tableData.TableSql%>&curPage=<%=curPage%>",
                "columns": [
                    <%=tableData.GenerateJSTableColumns()%>
                ],

                autoWidth: false,
                select: true,
                colReorder: {
                    realtime: false
                },
                colResize: {
                    "tableWidthFixed": true
                },
                stateSave: true,
                scrollY: ($(window).height() - 125) + "px",
                scrollX: true,
                scrollCollapse: false,
                lengthMenu: [
                    [30, 100, 200, 500],
                    ['30 строк', '100 строк', '200 строк', '500 строк']
                ],
                pagingType6: "simple",
                buttons: [
                    { extend: 'create', editor: editor, className: 'btn-sm', },
                    { extend: 'edit', editor: editor, className: 'btn-sm', },
                    { extend: 'remove', editor: editor, className: 'btn-sm', },
                    {
                        extend: 'collection',
                        text: 'Настройка таблицы',
                        buttons: [
                            {
                                extend: 'colvis',
                                text: 'Видимость столбцов',
                                postfixButtons: ['colvisRestore']
                            },
                            {
                                extend: 'pageLength',
                                text: 'Записей на страницу'
                            },
                            {
                                text: 'Сбросить все настройки',
                                action: function (e, dt, node, config) {
                                    dt.state.clear();
                                    window.location.reload();
                                }
                            }
                        ],
                        className: "btn-sm",
                    },
                    {
                        text: 'Фильтр',
                        action: function (e, dt, node, config) {
                            $('#modalFilterForm').modal();
                        },
                        className: "btn-sm",
                    }

                ],
                language: {
                    url: '/content/DataTables-1.10.12/js/Russian.json'
                }
            });
            $(window).resize();
        });
    </script>
</asp:Content>