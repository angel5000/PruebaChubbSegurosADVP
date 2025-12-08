import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ComponentSettings } from '../../../../pages/Asegurados/components/manejo-asegurados/list.config';
import { AseguradosService } from '../../../../pages/Asegurados/components/services/servicios.service';
import Swal from 'sweetalert2';
import { SegurosService } from '../../../../pages/Seguros/Servicios/seguros.service';
import { ModalErrorsComponent } from './modal-errors/modal-errors.component';
import { firstValueFrom } from 'rxjs';
import { HttpClient } from '@angular/common/http';
declare var bootstrap: any;

@Component({
  selector: 'app-subir-archivos',
  standalone: true,
  imports: [ModalErrorsComponent],
  templateUrl: './subir-archivos.component.html',
  styleUrl: './subir-archivos.component.scss'
})
export class SubirArchivosComponent implements OnInit {
  selectedFile: File;
  fileSelected: boolean = false;
  component: any;
  allowedExtensions: any[] = ['xls', 'xlsx', 'txt'];
  acceptTypes = this.allowedExtensions.map(e => '.' + e).join(', ');
  @Output() buttonClick = new EventEmitter<void>();
  @Input() bandeja: string = "";
  displayErrorModal: boolean = false;
  erroresMultiples: string[] = [];
  @ViewChild(ModalErrorsComponent) modalErrores!: ModalErrorsComponent;
errores: any;
  usuario: string;

  constructor(public aseguradosserv: AseguradosService, private toastr: ToastrService,
    public segurosserv:SegurosService,private http: HttpClient) { }
  ngOnInit(): void {
    this.component = ComponentSettings
 this.usuario = (localStorage.getItem('usuario') ?? '').replace(/"/g, '');
  }

  onFileChange(event: any): void {
    if (event.target.files.length > 0) {
      const file: File = event.target.files[0];
      const fileExtension = file.name.split('.').pop()?.toLowerCase();

      if (this.allowedExtensions.includes(fileExtension || '')) {
        this.selectedFile = file;
        this.fileSelected = true;
      } else {
        this.fileSelected = false;
        this.showError('Formato de archivo no permitido. Solo se aceptan .xls y .xlsx.');
      }
    } else {
      this.fileSelected = false;
    }
  }
  formatGetInputs() {
    let str = "";
    if (this.component.filters.refresh) {
      const random = Math.random();
      str += `&refresh=${random}`;
      this.component.filters.refresh = false;
    }
    this.component.getInputs = str;
  }
  setGetInputsProviders(refresh: boolean) {
    this.component.filters.refresh = refresh;
    this.formatGetInputs()
  }

async getClientIp(): Promise<string> {
  try {

    const data = await firstValueFrom(
      this.http.get<{ ip: string }>('https://api.ipify.org?format=json')
    );
    return data.ip;
  } catch (error) {
    console.warn('No se pudo obtener la IP, usando default');
    return '0.0.0.0'; 
  }
}

  async upload(fileInput: HTMLInputElement) {
    const ipuser = await this.getClientIp();
    Swal.fire({
      title: `Se iniciara un registro masivo de datos`,
      text: "¿Desea continuar?",
      icon: "warning",
      showCancelButton: true,
      focusCancel: true,
      confirmButtonColor: 'rgb(210,155,253)',
      cancelButtonColor: 'rgb(79,109,253)',
      cancelButtonText: 'cancelar',
      confirmButtonText: 'OK',
      width: 430

    }).then((result) => {
      if (result.isConfirmed) {
        if (this.selectedFile) {
          if(this.bandeja=='seguros'){
            this.segurosserv.RegistroMasivoSeguros(this.selectedFile,  this.usuario,ipuser).subscribe(
              (response) => {
                if (response.isSucces) {
                  this.showSuccess(response.message)
                  console.log(response.message)
                  fileInput.value = '';
                  this.selectedFile = null;
                  this.fileSelected = false;
                  this.setGetInputsProviders(true)
                  return this.buttonClick.emit();
                } else {
                  this.toastr.warning(`Se encontraron ${response.messagemultiple.length} inconveniente/es. Revisa el archivo.`);
                  this.erroresMultiples = response.messagemultiple;
                  this.setGetInputsProviders(true)
                  setTimeout(() => {
                    this.modalErrores.open();
                  }, 0);
                }
              }
  
            );
          }else{
            this.aseguradosserv.RegistroMasivoAsegurados(this.selectedFile).subscribe(
              (response) => {
                if (response.isSucces) {
                  this.showSuccess(response.message)
                  console.log(response.message)
                  fileInput.value = '';
                  this.selectedFile = null;
                  this.fileSelected = false;
                  this.setGetInputsProviders(true)
                  return this.buttonClick.emit();
                } else {
                  this.toastr.warning(`Se encontraron ${response.messagemultiple.length} inconveniente/es. Revisa el archivo.`);
                  this.erroresMultiples = response.messagemultiple;
                  this.setGetInputsProviders(true)
                  setTimeout(() => {
                    this.modalErrores.open();
                  }, 0);
                }
              }
  
            );
          }
          
        } else {
          console.error('No se ha seleccionado ningún archivo');
        }
      }
    })
  }

  showSuccess(mensaje: string) {
    this.toastr.success(mensaje, 'Éxito');
  }
  showError(mensaje: string) {
    this.toastr.error(mensaje, 'Error');
  }
}
