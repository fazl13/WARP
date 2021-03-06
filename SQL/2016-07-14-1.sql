/****** Script for SelectTopNRows command from SSMS  ******/
SELECT  a.[id]
      ,a.[num_doc]
      ,a.[date_doc]
      ,b.[name] as doctree
	  ,p.name as person
  FROM [Archive].[dbo].[zao_stg_archive] a
  left join [Archive].[dbo].[_doctree] b on b.id = a.id_doctree
  left join [Archive].[dbo].[zao_stg_person] p on p.id = a.id_perf
  where a.del=0 and a.id_perf in (791,856,858,870,895,924,928,1149,1257,1601,1857,1867,2026,2160,2172,2187,2220,2256,2272,2288,2294,2351,2444,2506,2524,2530,2532,2593,2596,2608,2609,2614,2647,2664,2739,2740,2772,2787,2887,2908,2930,2946,3014,3069,3074,3075,3079)