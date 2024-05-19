create table role_endpoint (
   role_endpoint_id     serial               not null,
   role_id              int4                 null,
   endpoint_id          int4                 null,
   created_date         timestamp without time zone                 null,
   updated_date         timestamp without time zone                 null,
   constraint pk_role_endpoint primary key (role_endpoint_id)
);

create unique index role_endpoint_pk on role_endpoint (
role_endpoint_id
);