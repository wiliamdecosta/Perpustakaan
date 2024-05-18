/*==============================================================*/
/* DBMS name:      PostgreSQL 8                                 */
/* Created on:     17/05/2024 12:25:31                          */
/*==============================================================*/


drop index book_pk;

drop table book;

drop index user_pk;

drop table "user";

/*==============================================================*/
/* Table: book                                                  */
/*==============================================================*/
create table book (
   book_id              serial               not null,
   title                varchar(255)         not null,
   author               varchar(100)         not null,
   description          text                 null,
   published_date       date                 null,
   created_date         date                 null,
   updated_date         date                 null,
   created_by           varchar(50)          null,
   updated_by           varchar(50)          null,
   constraint pk_book primary key (book_id)
);

/*==============================================================*/
/* Index: book_pk                                               */
/*==============================================================*/
create unique index book_pk on book (
book_id
);

/*==============================================================*/
/* Table: "user"                                                */
/*==============================================================*/
create table "user" (
   user_id              serial               not null,
   name                 varchar(50)          not null,
   email                varchar(50)          not null,
   password             varchar(255)         not null,
   created_date         date                 null,
   updated_date         date                 null,
   refresh_token        text                 null,
   refresh_token_expiry_time date                 null,
   constraint pk_user primary key (user_id)
);

/*==============================================================*/
/* Index: user_pk                                               */
/*==============================================================*/
create unique index user_pk on "user" (
user_id
);

