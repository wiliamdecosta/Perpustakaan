create table endpoint (
   endpoint_id          serial               not null,
   path_route           varchar(500)         not null,
   method               varchar(15)          not null,
   description          text                 null,
   created_date         timestamp without time zone                 null,
   updated_date         timestamp without time zone                 null,
   constraint pk_endpoint primary key (endpoint_id)
);

create unique index endpoint_pk on endpoint (
endpoint_id
);