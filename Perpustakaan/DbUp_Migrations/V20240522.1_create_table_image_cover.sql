create table image_cover (
   image_cover_id       serial               not null,
   book_id              int4                 null,
   file_name            varchar(100)         not null,
   original_file_name   varchar(100)         not null,
   file_path            varchar(500)         null,
   created_date         timestamp without time zone                 not null,
   updated_date         timestamp without time zone                 null,
   created_by           varchar(50)          null,
   updated_by           varchar(50)          null,
   constraint pk_image_cover primary key (image_cover_id)
);

create unique index image_cover_pk on image_cover (
image_cover_id
);