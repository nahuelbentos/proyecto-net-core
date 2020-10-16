import { Avatar, Button, Drawer, IconButton, List, ListItem, ListItemText, makeStyles, Toolbar, Typography } from '@material-ui/core';
import React, { useState } from 'react';
import FotoUsuarioTemp from '../../../logo.svg';
import { useStateValue } from '../../../context/store';
import { LeftMenu } from './leftMenu';
import { RightMenu } from './rightMenu';
import { withRouter } from 'react-router-dom';

const useStyles = makeStyles((theme) => ({
  seccionDesktop: {
    display: 'none',
    [theme.breakpoints.up('md')]: {
      display: 'flex',
    },
  },
  seccionMobile: {
    display: 'flex',
    [theme.breakpoints.up('md')]: {
      display: 'none',
    },
  },
  grow: {
    flexGrow: 1,
  },
  avatarSize: {
    width: 40,
    height: 40,
  },
  list: {
    width: 250,
  },
  listItemText: {
    fontSize: '14px',
    fontWeight: 600,
    paddingLeft: '15px',
    color: '#212121',
  },
}));

const BarSession = (props) => {
  const classes = useStyles();

  const [{ sesionUsuario }, dispatch] = useStateValue();

  const [openMenuLeft, setOpenMenuLeft] = useState(false);
  const closeLeftMenu = () => setOpenMenuLeft(false);
  const openMenuLeftAction = () => setOpenMenuLeft(true);

  const [openMenuRight, setOpenMenuRight] = useState(false);
  const closeRightMenu = () => setOpenMenuRight(false);
  const openMenuRightAction = () => setOpenMenuRight(true);

  const salirSessionApp = () => {
    localStorage.removeItem('token_seguridad');
    props.history.push('/auth/login');
  };

  return (
    <React.Fragment>
      <Drawer open={openMenuLeft} onClose={closeLeftMenu} anchor="left">
        <div className={classes.list} onKeyDown={closeLeftMenu} onClick={closeLeftMenu}>
          <LeftMenu classes={classes}></LeftMenu>
        </div>
      </Drawer>

      <Drawer open={openMenuRight} onClose={closeRightMenu} anchor="right">
        <div role="button" onKeyDown={closeRightMenu} onClick={closeRightMenu}>
          <RightMenu classes={classes} salirSession={salirSessionApp} usuario={sesionUsuario ? sesionUsuario.usuario : null}></RightMenu>
        </div>
      </Drawer>

      <Toolbar>
        <IconButton color="inherit" onClick={openMenuLeftAction}>
          <i className="material-icons">menu</i>
        </IconButton>

        <Typography variant="h6">Cursos Online</Typography>
        <div className={classes.grow}></div>
        <div className={classes.seccionDesktop}>
          <Button color="inherit"> Salir</Button>
          <Button color="inherit"> {sesionUsuario ? sesionUsuario.usuario.nombreCompleto : ''} </Button>
          <Avatar src={FotoUsuarioTemp}></Avatar>
        </div>
        <div className={classes.seccionMobile}>
          <IconButton color="inherit" onClick={openMenuRightAction}>
            <i className="material-icons"> more_vert </i>
          </IconButton>
        </div>
      </Toolbar>
    </React.Fragment>
  );
};

export default withRouter(BarSession);
