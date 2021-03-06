/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 
     
      a.[date_reg]
     
      ,a.[doccount]
      ,u.name as sig
      ,[whensign]
      ,[toprocess]
  FROM [Archive].[dbo].[_complectnew] a
  left join [Archive].[dbo].[_user] u on u.id =  a.id_sign
  where a.del=0 and a.id_sign in 
  (
	SELECT  [id]
      
  FROM [Archive].[dbo].[_user]
  where 
  name like 'Абрамов Валентин Витальевич' or
name like 'Белоконев Александр Михайлович' or
name like 'Бигарий Олеся Владимировна' or
name like 'Бобров Дмитрий Олегович' or
name like 'Бородин Владимир Александрович' or
name like 'Васюшкин Георгий Николаевич' or
name like 'Ветренко Роман Александрович' or
name like 'Волгин Михаил Борисович' or
name like 'Всеволодова Светлана Юрьевна' or
name like 'Габренайте Зита Павловна' or
name like 'Гзелишвили Денис Сергеевич' or
name like 'Голованов Илья Валерьевич' or
name like 'Головина Вера Георгиевна' or
name like 'Гурьянов Александр Владимирович' or
name like 'Гурьянов Андрей Владимирович' or
name like 'Данилин Антон Иванович' or
name like 'Дементьев Андрей Александрович' or
name like 'Димитриев Евгений Николаевич' or
name like 'Доленко Мария Васильевна' or
name like 'Евстратикова Татьяна Юрьевна' or
name like 'Жукова Людмила Александровна' or
name like 'Завизион Андрей Валерьевич' or
name like 'Зубков Иван Валентинович' or
name like 'Зуева Светлана Михайловна' or
name like 'Ильминский Михаил Викторович' or
name like 'Кабиров Булат Рифович' or
name like 'Килин Евгений Валерьевич' or
name like 'Кречетников Никита Александрович' or
name like 'Крученицкая Татьяна Григорьевна' or
name like 'Кузьмин Алексей Валерьевич' or
name like 'Кулямин Михаил Юрьевич' or
name like 'Курбатова Алена Владимировна' or
name like 'Лакеева Татьяна Викторовна' or
name like 'Латкина Валерия Владимировна' or
name like 'Леонтьева Татьяна Алексеевна' or
name like 'Литвиненко Анастасия Юрьевна' or
name like 'Литвинов Петр Яковлевич' or
name like 'Ломаков Александр Михайлович' or
name like 'Макарова Кира Викторовна' or
name like 'Малова Татьяна Владимировна ' or
name like 'Малышев Андрей Владимирович' or
name like 'Мальцев Денис Олегович' or
name like 'Маннанова Ляйсан Рашитовна' or
name like 'Марченко Дмитрий Алексеевич' or
name like 'Марченко Дмитрий Валерьевич' or
name like 'Мацагова Мария Умаровна' or
name like 'Мисюра Людмила Геннадьевна' or
name like 'Митрофанов Владимир Николаевич' or
name like 'Митрофанов Юрий Александрович' or
name like 'Никишин Кирилл Александрович' or
name like 'Носова Наталья Ивановна' or
name like 'Панов Максим Николаевич' or
name like 'Пономарев Тимур Алексеевич' or
name like 'Попцова Инзиля Айратовна' or
name like 'Разумова Марина Васильевна' or
name like 'Рыкунов Илья Петрович' or
name like 'Салицкая Ольга Владимировна' or
name like 'Самойлов Александр Сергеевич' or
name like 'Самоявчев Александр Николаевич' or
name like 'Свиридонов Михаил Александрович' or
name like 'Серов Станислав Александрович' or
name like 'Симаковский Валентин Викторович' or
name like 'Синкевич Людмила Юрьевна' or
name like 'Сорокин Дмитрий Владимирович' or
name like 'Сорокин Николай Васильевич' or
name like 'Сукачева Евгения Александровна' or
name like 'Телицын Виктор Александрович' or
name like 'Ткаченко Галина Леонидовна' or
name like 'Тренченко Денис Алексеевич' or
name like 'Трошенков Сергей Валерьевич' or
name like 'Трошенкова Юлия Николаевна' or
name like 'Тугарин Антон Сергеевич' or
name like 'Тяпкин Александр Андреевич' or
name like 'Улюшкин Алексей Владимирович' or
name like 'Утяшева Светлана Николаевна' or
name like 'Фабрикатов Игорь Леонидович' or
name like 'Хрипунова Ольга Александровна' or
name like 'Черных Алина Сергеевна' or
name like 'Чирикин Михаил Львович' or
name like 'Чичиланов Виктор Вениаминович' or
name like 'Шиков Юрий Алексеевич' or
name like 'Шматков Станислав Эдуардович' or
name like 'Шукюрова Мария Юрьевна'
  )