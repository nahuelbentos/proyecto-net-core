import { Divider, List, ListItem, ListItemText } from '@material-ui/core';
import React from 'react';
import { Link } from 'react-router-dom';

export const LeftMenu = ({ classes }) => (
  <div className={classes.list}>
    <List>
      <ListItem button component={Link} to="/auth/perfil">
        <i className="material-icons"> account_box</i>
        <ListItemText classes={{ primary: classes.listItemText }} primary="Perfil"></ListItemText>
      </ListItem>
    </List>

    <Divider />

    <List>
      <ListItem button component={Link} to="/curso/nuevo">
        <i className="material-icons"> add_box</i>
        <ListItemText classes={{ primary: classes.listItemText }} primary="Crear Curso"></ListItemText>
      </ListItem>
      <ListItem button component={Link} to="/curso/lista">
        <i className="material-icons"> menu_book</i>
        <ListItemText classes={{ primary: classes.listItemText }} primary="Lista Cursos"></ListItemText>
      </ListItem>
    </List>

    <Divider />

    <List>
      <ListItem button component={Link} to="/instructor/nuevo">
        <i className="material-icons"> person_add</i>
        <ListItemText classes={{ primary: classes.listItemText }} primary="Crear Intructror"></ListItemText>
      </ListItem>
      <ListItem button component={Link} to="/instructor/lista">
        <i className="material-icons"> people</i>
        <ListItemText classes={{ primary: classes.listItemText }} primary="Lista Intructrores"></ListItemText>
      </ListItem>
    </List>
  </div>
);
