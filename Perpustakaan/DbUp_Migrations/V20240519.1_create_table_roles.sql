create table roles (
   role_id              serial               not null,
   name                 varchar(50)          not null,
   created_date         timestamp without time zone                 null,
   updated_date         timestamp without time zone                 null,
   constraint pk_roles primary key (role_id)
);

create unique index role_pk on roles (
role_id
);