create  index r1_fk on user_role (
user_id
);

create  index r2_fk on user_role (
role_id
);

create  index r3_fk on role_endpoint (
role_id
);

create  index r4_fk on role_endpoint (
endpoint_id
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