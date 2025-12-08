import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, Observable, of, throwError } from 'rxjs';
import { BaseResponse } from '../../../shared/models/BaseApiResponse';
import { environment as env } from '../../../../enviroments/environment';
import { endpoint as end } from '../../../shared/apis/endpoints';
import { SegurosRequest, SegurosResponseID } from '../Models/seguros.interface';

@Injectable({
  providedIn: 'root'
})

export class SegurosService {

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  showSuccess(mensaje: string) {
    this.toastr.success(mensaje, 'Éxito');
  }

  showError(mensaje: string) {
    this.toastr.error(mensaje, 'Error');
  }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem("token")?.replace(/"/g, '');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  Obtenerdatos(): Observable<BaseResponse[]> {
    return this.http.get<any>(env.apiseguros + end.CONSULTASEGUROS, { headers: this.getAuthHeaders() }).pipe(

      map((res) => res),
      catchError((error) => {
        this.toastr.error('Error al obtener los datos');
        return throwError(() => error);
      })
    );
  }

  RegistrarSeguro(request: SegurosRequest): Observable<BaseResponse> {
    return this.http.post<BaseResponse>(env.apiseguros + end.REGISTRARASEGURO, request, { headers: this.getAuthHeaders() });
  }

  ActualizarSeguro(id: number, request: SegurosRequest): Observable<BaseResponse> {
    return this.http.put<BaseResponse>(`${env.apiseguros}${end.ACTUALIZARASEGURO}${id}`, request, { headers: this.getAuthHeaders() });
  }

  SeguroPorId(id: number): Observable<BaseResponse> {
    return this.http.get<SegurosResponseID>(`${env.apiseguros}${end.CONSULTASEGUROSID}${id}`, { headers: this.getAuthHeaders() }).pipe(
      map((res) => res),
      catchError((error) => {
        this.showError(error);
        return of(error);
      }));
  }


  AseguradosPorseguro(id: number): Observable<BaseResponse> {
    return this.http.get<SegurosResponseID>(`${env.apiseguros}${end.ASEGURADOSPORSEGUDOR}${id}`, { headers: this.getAuthHeaders() }).pipe(
      map((res) => res),
      catchError((error) => {
        this.showError(error);
        return of(error);
      }));
  }

  EliminarSeguro(id: Number, request: SegurosRequest): Observable<BaseResponse> {
    const requestURL = `${env.apiseguros}${end.ELIMINARSEGURO}${id}`;

    return this.http.request('DELETE', requestURL, { body: request, headers: this.getAuthHeaders() })
      .pipe(
        map(res => res as BaseResponse),
        catchError(err => {
          this.showError(err);
          return of(err);
        })
      );
  }

  RegistroMasivoSeguros(file: File): Observable<BaseResponse> {
    const token = localStorage.getItem("token")?.replace(/"/g, '');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    const formData: FormData = new FormData();
    formData.append('archivo', file, file.name);
    return this.http.post<BaseResponse>(`${env.apiseguros}${end.REGISTRARSEGUROMASIVO}`, formData, { headers });
  }

}