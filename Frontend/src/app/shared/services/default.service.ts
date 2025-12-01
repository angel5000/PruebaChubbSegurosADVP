import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export abstract class DefaultService {
  constructor() { }
  /**
   * Metodo general para las consultas de datos, se implementa en la estructura de la tabla reutilizable
   */
  abstract Obtenerdatos(): Observable<any>;
  
}