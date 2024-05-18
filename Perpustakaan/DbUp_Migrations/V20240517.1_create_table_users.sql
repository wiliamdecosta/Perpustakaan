create table users (
   user_id              serial               not null,
   name                 varchar(100)          not null,
   email                varchar(100)          not null,
   password             varchar(255)         not null,
   created_date         timestamp without time zone                 null,
   updated_date         timestamp without time zone                 null,
   refresh_token        text                 null,
   refresh_token_expiry_time timestamp without time zone                 null,
   constraint pk_users primary key (user_id)
);

create unique index users_pk on users (
user_id
);