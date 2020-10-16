import { AppBar } from '@material-ui/core';
import React from 'react';
import { useStateValue } from '../../context/store';
import BarSession from './bar/BarSession';

const AppNavbar = () => {
  const [{ sesionUsuario }, dispatch] = useStateValue();

  return sesionUsuario ? (
    sesionUsuario.autenticado ? (
      <AppBar position="static">
        <BarSession />
      </AppBar>
    ) : null
  ) : null;
  // return (
  //   <AppBar position="static">
  //     <BarSession></BarSession>
  //   </AppBar>
  // );
};

export default AppNavbar;
