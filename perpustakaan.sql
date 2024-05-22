/*==============================================================*/
/* DBMS name:      PostgreSQL 8                                 */
/* Created on:     22/05/2024 15:28:51                          */
/*==============================================================*/


drop index book_pk;

drop table books;

drop index endpoint_pk;

drop table endpoints;

drop index r5_fk;

drop index image_cover_pk;

drop table image_cover;

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
/* Table: books                                                 */
/*==============================================================*/
create table books (
   book_id              serial               not null,
   title                varchar(255)         not null,
   author               varchar(100)         not null,
   description          text                 null,
   published_date       date                 null,
   created_date         date                 null,
   updated_date         date                 null,
   created_by           varchar(50)          null,
   updated_by           varchar(50)          null,
   constraint pk_books primary key (book_id)
);

/*==============================================================*/
/* Index: book_pk                                               */
/*==============================================================*/
create unique index book_pk on books (
book_id
);

/*==============================================================*/
/* Table: endpoints                                             */
/*==============================================================*/
create table endpoints (
   endpoint_id          serial               not null,
   path                 varchar(500)         not null,
   method               varchar(15)          not null,
   description          text                 null,
   created_date         date                 null,
   updated_date         date                 null,
   constraint pk_endpoints primary key (endpoint_id)
);

/*==============================================================*/
/* Index: endpoint_pk                                           */
/*==============================================================*/
create unique index endpoint_pk on endpoints (
endpoint_id
);

/*==============================================================*/
/* Table: image_cover                                           */
/*==============================================================*/
create table image_cover (
   image_cover_id       serial               not null,
   book_id              int4                 null,
   file_name            varchar(100)         not null,
   file_path            varchar(100)         null,
   created_date         date                 not null,
   updated_date         date                 null,
   created_by           varchar(50)          null,
   updated_by           varchar(50)          null,
   constraint pk_image_cover primary key (image_cover_id)
);

/*==============================================================*/
/* Index: image_cover_pk                                        */
/*==============================================================*/
create unique index image_cover_pk on image_cover (
image_cover_id
);

/*==============================================================*/
/* Index: r5_fk                                                 */
/*==============================================================*/
create  index r5_fk on image_cover (
book_id
);

/*==============================================================*/
/* Table: roles                                                 */
/*==============================================================*/
create table roles (
   role_id              serial               not null,
   name                 varchar(50)          not null,
   created_date         date                 null,
   updated_date         date                 null,
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
   created_date         date                 null,
   updated_date         date                 null,
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
   created_date         date                 null,
   updated_date         date                 null,
   refresh_token        text                 null,
   refresh_token_expiry_time date                 null,
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
   created_date         date                 null,
   updated_date         date                 null,
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

alter table image_cover
   add constraint fk_image_co_r5_books foreign key (book_id)
      references books (book_id)
      on delete restrict on update restrict;

alter table role_endpoint
   add constraint fk_role_end_r3_roles foreign key (role_id)
      references roles (role_id)
      on delete restrict on update restrict;

alter table role_endpoint
   add constraint fk_role_end_r4_endpoint foreign key (endpoint_id)
      references endpoints (endpoint_id)
      on delete restrict on update restrict;

alter table user_role
   add constraint fk_user_rol_r1_users foreign key (user_id)
      references users (user_id)
      on delete restrict on update restrict;

alter table user_role
   add constraint fk_user_rol_r2_roles foreign key (role_id)
      references roles (role_id)
      on delete restrict on update restrict;

