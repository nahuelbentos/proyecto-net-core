import React, { useState, useEffect } from 'react';

export default function ControlTyping(texto, delay) {
  const [textoValor, setTextoValor] = useState();

  useEffect(() => {
    const manejador = setTimeout(() => {
      setTextoValor(texto);
    }, delay);

    return () => {
      clearTimeout(manejador);
    };
  }, [texto]);

  return textoValor;
}

/*
import React, { useEffect, useState } from 'react';

const controlTyping = (texto, delay) => {
  const [textoValor, setTextoValor] = useState();
  useEffect(() => {
    const manejador = setTimeout(() => setTextoValor(texto), delay);

    return () => clearTimeout();
  }, [texto]);
  return textoValor;
};

export default controlTyping;
*/
