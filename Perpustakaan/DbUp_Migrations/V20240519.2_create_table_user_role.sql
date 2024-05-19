create table user_role (
   user_role_id         serial               not null,
   user_id              int4                 null,
   role_id              int4                 null,
   created_date         timestamp without time zone                 null,
   updated_date         timestamp without time zone                 null,
   constraint pk_user_role primary key (user_role_id)
);

create unique index user_role_pk on user_role (
user_role_id
);