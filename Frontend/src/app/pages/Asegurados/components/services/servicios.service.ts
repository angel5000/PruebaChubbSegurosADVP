import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, Observable, of, throwError } from 'rxjs';
import { BaseResponse } from '../../../../shared/models/BaseApiResponse';
import { environment as env } from '../../../../../enviroments/environment';  
import { endpoint as end} from '../../../../shared/apis/endpoints'; 
import { Asegurados, AseguradosRequest, Aseguramiento } from '../../Model/asegurados.interface';
@Injectable({
  providedIn: 'root'
})
export class AseguradosService {

  constructor(private http: HttpClient,private toastr: ToastrService) { }

  showSuccess(mensaje: string) {
    this.toastr.success(mensaje, 'Éxito');
  }

  showError(mensaje: string) {
    this.toastr.error(mensaje, 'Error');
  }

  Obtenerdatos(): Observable<BaseResponse[]> {
    return this.http.get<any>(env.apiseguros+end.CONSULTAASEGURAMIENTO).pipe(
      map((res) => res), 
      catchError((error) => {
        this.toastr.error('Error al obtener los seguros');
        return throwError(() => error);
      })
    );
  }

  ObtenerSegdisponibles(edad:number): Observable<BaseResponse> {
    return this.http.get<any>(`${env.apiseguros}${end.SEGUROSDISPONIBLES}${edad}`).pipe(
      map((res) => res), 
      catchError((error) => {
        this.toastr.error('Error al obtener los seguros disponibles');
        return throwError(() => error);
      })
    );
  }

  RegistrarAsegurado(request: AseguradosRequest): Observable<BaseResponse> {
    return this.http.post<BaseResponse>(env.apiseguros+end.REGISTRARASEGURADO, request);
  }

  RegistrarAseguramiento(request: Aseguramiento): Observable<BaseResponse> {
    return this.http.post<BaseResponse>(env.apiseguros+end.REGISTRARASEGURAMIENT, request);
  }

  ActualizarAsegurado(id:number,request: AseguradosRequest): Observable<BaseResponse> {
    return this.http.put<BaseResponse>(`${env.apiseguros}${end.ACTUALIZARASEGURADOS}${id}`, request);
  }

  AseguradoPorId(id: number): Observable<BaseResponse> {
    return this.http.get<Asegurados>(`${env.apiseguros}${end.CONSULTASEGURADOSID}${id}`).pipe(
      map((res) => res),
      catchError((error) => {
        this.showError(error);
        return of(error);  
      }));
    }
  
    EliminarAsegurado(id:Number):Observable<BaseResponse>{
      const requestURL=  `${env.apiseguros}${end.ELIMINARASEGURADO}${id}`;
      return this.http.delete(requestURL).pipe( map((res) => res),
      catchError((error) => {
        this.showError(error);
        return of(error);  
      }));
    }

    EliminarAseguramiento(id:Number):Observable<BaseResponse>{
      const requestURL=  `${env.apiseguros}${end.ELIMINARAASEGURAMIENTO}${id}`;
      return this.http.delete(requestURL).pipe( map((res) => res),
      catchError((error) => {
        this.showError(error);
        return of(error);  
      }));
    }

   RegistroMasivoAsegurados(file: File): Observable<BaseResponse> {
      const formData: FormData = new FormData();
      formData.append('archivo', file, file.name);
      return this.http.post<BaseResponse>(`${env.apiseguros}${end.ASEGURADOSMASIVOS}`, formData);
  }

}
