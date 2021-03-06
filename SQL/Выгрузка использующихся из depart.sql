/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/
use [testbase];

select distinct a.id_departs, b.n_depart from (
SELECT [id_departs] FROM [testbase].[dbo].[zao_stg_a_departs] group by id_departs
Union ALL 
SELECT [id_departs] FROM [testbase].[dbo].[ooo_ag_a_departs] group by id_departs
Union ALL 
SELECT [id_departs] FROM [testbase].[dbo].[asm_a_departs] group by id_departs
Union ALL 
SELECT [id_departs] FROM [testbase].[dbo].[ooo_aps_east_a_departs] group by id_departs
Union ALL 
SELECT [id_departs] FROM [testbase].[dbo].[ooo_aps_ngm_a_departs] group by id_departs
Union ALL 
SELECT [id_departs] FROM [testbase].[dbo].[ooo_aps_north_a_departs] group by id_departs
Union ALL 
SELECT [id_departs] FROM [testbase].[dbo].[ooo_aps_west_a_departs] group by id_departs
Union ALL 
SELECT [id_departs] FROM [testbase].[dbo].[ooo_apvs_a_departs] group by id_departs
Union ALL 
SELECT [id_departs] FROM [testbase].[dbo].[ooo_asm_a_departs] group by id_departs
Union ALL 
SELECT [id_departs] FROM [testbase].[dbo].[ooo_stg_autotrans_a_departs] group by id_departs
) a Left join [testbase].[dbo].[zao_stg_departs] b ON a.id_departs = b.id

--order by id_departs
