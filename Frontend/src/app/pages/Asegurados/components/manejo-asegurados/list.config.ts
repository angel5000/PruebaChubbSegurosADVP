
import { SearchOptions } from "../../../../shared/models/SearchOptions.interface";
import { GenericValidators } from "../../../../shared/validators/generic-validators";

const searchOptions: SearchOptions[] = [
  {
    label: "Cedula",
    value: 1,
    placeholder: "Buscar cedula",
    validation: [GenericValidators.numeric],
    validation_desc: "Solo se permite numeros",
    min_lenght: 2,
  },

]
function getTableColumns() {
  return [
    {
      label: "ID",
      csslabel: ["font-bold", "text-sm"],
      property: "idasegurados",
      cssProperty: ["font-semibold", "text-sm", "text-left"],
      type: "number",
      sticky: true,
      sort: true,
      sortProperty: "idasegurados", 
      visible: false,
      download: true,
    },
    {
      label: "Cedula",
      csslabel: ["font-bold", "text-sm"],
      property: "cedula",
      cssProperty: ["font-semibold", "text-sm", "text-left"],
      type: "text",
      sticky: true,
      sort: true,
      sortProperty: "cedula", 
      visible: true,
      download: true,
    },
    {
      label: "Nombre Completo",
      csslabel: ["font-bold", "text-sm"],
      property: "nmbrcompleto",
      cssProperty: ["font-semibold", "text-sm", "text-left"],
      type: "text",
      sticky: false,
      sort: true,
      sortProperty: "nmbrcompleto", 
      visible: true,
      download: true,
    },

    {
      label: "Edad",
      property: "edad",
      type: "number",
      csslabel: ["font-bold", "text-sm"],
      cssProperty: ["font-semibold", "text-sm", "text-left"],
      sticky: false,
      sort: true,
      visible: true,
      download: true,
    },
    {
      label: "ID",
      property: "idusrseguros",
      type: "number",
      csslabel: ["font-bold", "text-sm"],
      cssProperty: ["font-semibold", "text-sm", "text-left"],
      sticky: false,
      sort: true,
      visible: false,
      download: true,
    },
    {
      label: "Nombre del Seguro",
      property: "nmbrseguro",
      type: "text",
      csslabel: ["font-bold", "text-sm"],
      cssProperty: ["font-semibold", "text-sm", "text-left"],
      sticky: false,
      sort: true,
      visible: true,
      download: true,
    },

    {
      label: "Cod. del seguro",
      property: "codseguro",
      type: "text",
      csslabel: ["font-bold", "text-sm"],
      cssProperty: ["font-semibold", "text-sm", "text-left"],
      sticky: false,
      sort: true,
      visible: true,
      download: true,
    },

    {
      label: "Suma del seguro",
      property: "sumasegurada",
      type: "currency",
      csslabel: ["font-bold", "text-sm"],
      cssProperty: ["font-semibold", "text-sm", "text-left"],
      sticky: false,
      sort: true,
      visible: true,
      download: true,
    },

    {
      label: "Prima",
      property: "prima",
      type: "currency",
      csslabel: ["font-bold", "text-sm"],
      cssProperty: ["font-semibold", "text-sm", "text-left"],
      sticky: false,
      sort: true,
      visible: true,
      download: true,
    },

    {
      label: "Fecha de Contrato",
      property: "fechacontrataseguro",
      type: "text",
      csslabel: ["font-bold", "text-sm"],
      cssProperty: ["font-semibold", "text-sm", "text-left"],
      sticky: false,
      sort: true,
      visible: true,
      download: true,
    },
    {
      label: "",
      csslabel: [],
      cssProperty: ["bi bi-pen fs-5"],
      cssSubProperty: ['bi bi-pen fs-5'],
      property: "icEdit",
      type: "icon",
      tooltip: "editar",
      action: "edit",
      sticky: false,
      sort: false,
      visible: true,
      download: false

    },
    {
      label: "",
      csslabel: [],
      cssProperty: [''],
      property: "eye",
      cssSubProperty: ["bi bi-eye fs-5"],
      type: "icon",
      action: "ver",
      visible: Permisos.consultar,
      tooltip: "ver Seguros",
      sticky: false,
      sort: false,
      download: false
    },
    {
      label: "",
      csslabel: [],
      cssProperty: [''],
      property: "plus",
      cssSubProperty: ["bi bi-clipboard2-plus fs-5"],
      type: "icon",
      action: "agregar",
      visible: true,
      tooltip: "Agregar Seguros",
      sticky: false,
      sort: false,
      download: false

    },
    {
      label: "",
      csslabel: [],
      cssProperty: [''],
      property: "icDelete",
      cssSubProperty: ["bi bi-trash3 fs-5"],
      type: "icon",
      action: "remove",
      tooltip: "eliminar",
      sticky: false,
      sort: false,
      visible: true,
      download: false

    }
  ];
}

const filters = {
  numFilter: "",
  textFilter: "",
  stateFilter: null,
  startDate: null,
  endDate: null,
  refresh: false
}


const Permisos = {
  consultar: true,
};

export class actualizarPermiso {

  PermisoConsultar(valor: boolean) {
    Permisos.consultar = valor;
    ComponentSettings.tableColumns = getTableColumns();
  }
}

const getInputs: string = "";

export const ComponentSettings = {
  Permisos: Permisos,
  tableColumns: getTableColumns(),
  getInputs,
  filters: filters,
  searchOptions: searchOptions,
}
