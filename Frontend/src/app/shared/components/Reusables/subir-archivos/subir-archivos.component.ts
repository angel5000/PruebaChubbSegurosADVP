import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ComponentSettings } from '../../../../pages/Asegurados/components/manejo-asegurados/list.config';
import { AseguradosService } from '../../../../pages/Asegurados/components/services/servicios.service';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-subir-archivos',
  standalone: true,
  imports: [],
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
  constructor(public aseguradosserv: AseguradosService, private toastr: ToastrService) { }
  ngOnInit(): void {
    this.component = ComponentSettings

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

  upload(fileInput: HTMLInputElement) {

    Swal.fire({
      title: `Esta carga asignara automaticamente los seguros a los usuarios segun su edad`,
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
              }
            }

          );
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
