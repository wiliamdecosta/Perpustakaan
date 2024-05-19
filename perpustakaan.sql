/*==============================================================*/
/* DBMS name:      PostgreSQL 8                                 */
/* Created on:     19/05/2024 01:55:55                          */
/*==============================================================*/


drop index book_pk;

drop table book;

drop index endpoint_pk;

drop table endpoint;

drop index role_pk;

drop table roles;

drop index r4_fk;

drop index r3_fk;

drop index role_endpoint_pk;

drop table role_endpoint;

drop index user_pk;

drop table users;

drop index r2_fk;

drop index r1_fk;

drop index user_role_pk;

drop table user_role;

/*==============================================================*/
/* Table: book                                                  */
/*==============================================================*/
create table book (
   book_id              serial               not null,
   title                varchar(255)         not null,
   author               varchar(100)         not null,
   description          text                 null,
   published_date       timestamp without time zone                 null,
   created_date         timestamp without time zone                 null,
   updated_date         timestamp without time zone                 null,
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
/* Table: endpoint                                              */
/*==============================================================*/
create table endpoint (
   endpoint_id          serial               not null,
   path_route           varchar(500)         not null,
   method               varchar(15)          not null,
   description          text                 null,
   created_date         timestamp without time zone                 null,
   updated_date         timestamp without time zone                 null,
   constraint pk_endpoint primary key (endpoint_id)
);

/*==============================================================*/
/* Index: endpoint_pk                                           */
/*==============================================================*/
create unique index endpoint_pk on endpoint (
endpoint_id
);

/*==============================================================*/
/* Table: roles                                                 */
/*==============================================================*/
create table roles (
   role_id              serial               not null,
   name                 varchar(50)          not null,
   created_date         timestamp without time zone                 null,
   updated_date         timestamp without time zone                 null,
   constraint pk_roles primary key (role_id)
);

/*==============================================================*/
/* Index: role_pk                                               */
/*==============================================================*/
create unique index role_pk on roles (
role_id
);

/*==============================================================*/
/* Table: role_endpoint                                         */
/*==============================================================*/
create table role_endpoint (
   role_endpoint_id     serial               not null,
   role_id              int4                 null,
   endpoint_id          int4                 null,
   created_date         timestamp without time zone                 null,
   updated_date         timestamp without time zone                 null,
   constraint pk_role_endpoint primary key (role_endpoint_id)
);

/*==============================================================*/
/* Index: role_endpoint_pk                                      */
/*==============================================================*/
create unique index role_endpoint_pk on role_endpoint (
role_endpoint_id
);

/*==============================================================*/
/* Index: r3_fk                                                 */
/*==============================================================*/
create  index r3_fk on role_endpoint (
role_id
);

/*==============================================================*/
/* Index: r4_fk                                                 */
/*==============================================================*/
create  index r4_fk on role_endpoint (
endpoint_id
);

/*==============================================================*/
/* Table: users                                                 */
/*==============================================================*/
create table users (
   user_id              serial               not null,
   name                 varchar(50)          not null,
   email                varchar(50)          not null,
   password             varchar(255)         not null,
   created_date         timestamp without time zone                 null,
   updated_date         timestamp without time zone                 null,
   refresh_token        text                 null,
   refresh_token_expiry_time timestamp without time zone                 null,
   constraint pk_users primary key (user_id)
);

/*==============================================================*/
/* Index: user_pk                                               */
/*==============================================================*/
create unique index user_pk on users (
user_id
);

/*==============================================================*/
/* Table: user_role                                             */
/*==============================================================*/
create table user_role (
   user_role_id         serial               not null,
   user_id              int4                 null,
   role_id              int4                 null,
   created_date         timestamp without time zone                 null,
   updated_date         timestamp without time zone                 null,
   constraint pk_user_role primary key (user_role_id)
);

/*==============================================================*/
/* Index: user_role_pk                                          */
/*==============================================================*/
create unique index user_role_pk on user_role (
user_role_id
);

/*==============================================================*/
/* Index: r1_fk                                                 */
/*==============================================================*/
create  index r1_fk on user_role (
user_id
);

/*==============================================================*/
/* Index: r2_fk                                                 */
/*==============================================================*/
create  index r2_fk on user_role (
role_id
);

alter table role_endpoint
   add constraint fk_role_end_r3_roles foreign key (role_id)
      references roles (role_id)
      on delete restrict on update restrict;

alter table role_endpoint
   add constraint fk_role_end_r4_endpoint foreign key (endpoint_id)
      references endpoint (endpoint_id)
      on delete restrict on update restrict;

alter table user_role
   add constraint fk_user_rol_r1_users foreign key (user_id)
      references users (user_id)
      on delete restrict on update restrict;

alter table user_role
   add constraint fk_user_rol_r2_roles foreign key (role_id)
      references roles (role_id)
      on delete restrict on update restrict;

