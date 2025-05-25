--CREATE TABLE [dbo].[marcas] (
--    [Id]    INT    primary key       IDENTITY (1, 1) NOT NULL,
--    [marca] NVARCHAR (50) not NULL,
    
--);

--set identity_insert marcas ON;
--insert into marcas (Id, marca) values
--(1,'BW'),
--(2,'Citroen'),
--(3,'Fiat'),
--(4,'Ford');
--set identity_insert marcas OFF;

--select * from marcas;



--create table clientes(
-- idcli int identity(1,1) primary key,
-- nome nvarchar(50) not null,
-- datanasc date,
-- idade as datediff(year,datanasc,getdate()),
-- categoria nvarchar(50) check(categoria in ('Alpha','Beta','Gama')),
-- tutor int foreign key references clientes(idcli) unique,
-- fotopath nvarchar(255)
--);  

--set identity_insert clientes ON;
--insert into clientes (idcli, nome, datanasc ,categoria,tutor)
--values
--(1,'Tio Patinhas', '1960-01-01','Alpha',NULL),
--(2,'Pato Donald', '1967-01-01','Beta',1),
--(3,'Mickey', '1975-01-01','Beta',4),
--(4,'Gastão', '1975-01-01','Alpha',2),
--(5,'Peninha', '1979-01-01','Gama',3);
--set identity_insert clientes OFF;

select * from marcas;

--create table carros(
--   idcarro int identity(10,1) Primary key,
--   marca int foreign key references marcas(id),
--   modelo nvarchar(50) not null,
--   ano int check(ano >= 1900 and ano <= year(getdate())),
--   phora decimal(10,2) check(phora >= 0)
--);


--insert into carros (marca, modelo, ano, phora)
--values
--(1,'BMW F4', 1970, 10.00),
--(2,'Saxo', 2010, 20.00),
--(3,'Punto', 2005, 15.00),
--(4,'Focus', 2015, 25.00);

--select * from carros inner join marcas on marcas.id = carros.marca;

--create table alugueres (
--  idal int identity(1,1) primary key,
--  idcli int foreign key references clientes(idcli),
--  idcarro int foreign key references carros(idcarro),
--  datainicio date not null default getdate(),
--  tempo decimal(10,2) check(tempo >= 0),
--  constraint ck_unico unique(idcli,idcarro,datainicio),
--  custo decimal(10,2) check(custo >= 0)

--);

--alter table clientes add foto nvarchar(255) null;

--alter table marcas add fotobin varbinary(max) null;

--insert into alugueres (idcli, idcarro)
--select c.idcli, ca.idcarro from clientes c, carros ca;

update alugueres set tempo = floor(rand() * 24);

GO
    create view vw_tombola
    as
    select rand() as sorte;
GO



--GO
-- create function fn_aleatorio(@minimo int, @maximo int)returns int
-- AS
-- BEGIN
--    declare @delta int = @maximo - @minimo+1;
--    return @minimo +  floor((select sorte from vw_tombola) * @delta); 
--  END
--GO

--update  alugueres set tempo=dbo.fn_aleatorio(1,24);

--select * from alugueres;

update alugueres set custo = a.tempo * ca.phora
from alugueres a inner join carros ca on a.idcarro = ca.idcarro;

go
 create trigger tr_alugueres on alugueres
 after insert, update
as
BEGIN
       update alugueres set custo = i.tempo * ca.phora
              from alugueres a inner join carros ca on a.idcarro = ca.idcarro
              inner join inserted  i on i.idal=a.idal;

END
go

insert into alugueres (idcli, idcarro,tempo,datainicio)
values (1,10,1, '2025-06-05');

select * from alugueres;