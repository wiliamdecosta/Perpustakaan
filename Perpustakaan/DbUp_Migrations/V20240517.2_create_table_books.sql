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

create unique index book_pk on book (
book_id
);