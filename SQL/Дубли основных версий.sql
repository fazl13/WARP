/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/
Select a.* from (SELECT count(*) as cnt, [id_archive]
  FROM [Archive].[dbo].[zao_stg_docversion] where main=1 and del=0
  group by id_archive ) a
  where a.cnt>1 