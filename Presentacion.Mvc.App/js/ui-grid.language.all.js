/*!
 * ui-grid - v4.8.1 - 2019-06-27
 * Copyright (c) 2019 ; License: MIT 
 */

(function() {
	angular.module('ui.grid').config(['$provide', function($provide) {
		$provide.decorator('i18nService', ['$delegate', function($delegate) {
			$delegate.add('ar', {
				"headerCell": {
					"aria": {
						"defaultFilterLabel": "??????? ???????",
						"removeFilter": "??? ???????",
						"columnMenuButtonLabel": "????? ???????"
					},
					"priority": "?????? : ",
					"filterLabel": "????? ???????? :"
				},
				"aggregate": {
					"label": "???????"
				},
				"groupPanel": {
					"description": "???? ??? ?????? ??? ?????? ?????? ??????"
				},
				"search": {
					"placeholder": "???  ...",
					"showingItems": "??????? ??????? :",
					"selectedItems": "??????? ??????? :",
					"totalItems": "??? ??????? :",
					"size": "??? ?????? :",
					"first": "??? ????",
					"next": "?????? ???????",
					"previous": "?????? ???????",
					"last": "?????? ???????"
				},
				"menu": {
					"text": "?????? ?????? :"
				},
				"sort": {
					"ascending": "????? ??????",
					"descending": "????? ??????",
					"none": "??? ???????",
					"remove": "??? ???????"
				},
				"column": {
					"hide": "????? ????"
				},
				"aggregation": {
					"count": "??? ??????: ",
					"sum": "???: ",
					"avg": "??????? ???????: ",
					"min": "??????: ",
					"max": "??????: "
				},
				"pinning": {
					"pinLeft": "????? ??????",
					"pinRight": "????? ??????",
					"unpin": "?? ???????"
				},
				"columnMenu": {
					"close": "???"
				},
				"gridMenu": {
					"aria": {
						"buttonLabel": "????? ??????"
					},
					"columns": "???????:",
					"importerTitle": "????? ???",
					"exporterAllAsCsv": "????? ?? ???????? ?(csv)",
					"exporterVisibleAsCsv": "????? ?? ???????? ??????? ? (csv)",
					"exporterSelectedAsCsv": "????? ?? ???????? ??????? ? (csv)",
					"exporterAllAsPdf": "????? ?? ???????? ?(pdf)",
					"exporterVisibleAsPdf": "????? ?? ???????? ??????? ? (pdf)",
					"exporterSelectedAsPdf": "????? ?? ???????? ??????? ? (pdf)",
					"clearAllFilters": "??? ?? ???????"
				},
				"importer": {
					"noHeaders": "????? ????? ??????? ??? ?????? ?? ???? ??? ??????",
					"noObjects": "Objects were not able to be derived, was there data in the file other than headers?",
					"invalidCsv": "????? ??? ???? ??? ??????? ? ?? ?? (CSV) ?????",
					"invalidJson": "????? ??? ???? ??? ??????? ? ?? ?? (JSON) ?????",
					"jsonNotArray": "Imported json file must contain an array, aborting."
				},
				"pagination": {
					"aria": {
						"pageToFirst": "?????? ??????",
						"pageBack": "????? ???????",
						"pageSelected": "?????? ???????",
						"pageForward": "?????? ???????",
						"pageToLast": "?????? ???????"
					},
					"sizes": "??? ??????? ?? ??????",
					"totalItems": "?????",
					"through": "???",
					"of": "??"
				},
				"grouping": {
					"group": "???",
					"ungroup": "?? ?????",
					"aggregate_count": "???? : ?????",
					"aggregate_sum": "???? : ??????",
					"aggregate_max": "???? : ??????",
					"aggregate_min": "???? : ?????",
					"aggregate_avg": "???? :??????? ",
					"aggregate_remove": "???? : ???"
				},
				"validate": {
					"error": "??? :",
					"minLength": "?????? ???? ?? ?? ??? ?? THRESHOLD ???.",
					"maxLength": "?????? ???? ?? ?? ???? ?? THRESHOLD ???.",
					"required": "????? ????"
				}
			});
			return $delegate;
		}]);
	}]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('bg', {
        headerCell: {
          aria: {
            defaultFilterLabel: '??????? ?? ????????',
            removeFilter: '???????? ??????',
            columnMenuButtonLabel: '???? ?? ????????'
          },
          priority: '?????????:',
          filterLabel: "?????? ?? ????????: "
        },
        aggregate: {
          label: '??????'
        },
        search: {
          placeholder: '???????...',
          showingItems: '???????? ??????:',
          selectedItems: '??????? ??????:',
          totalItems: '????:',
          size: '?????? ?? ??????????:',
          first: '????? ????????',
          next: '???????? ????????',
          previous: '???????? ????????',
          last: '???????? ????????'
        },
        menu: {
          text: '?????? ??????:'
        },
        sort: {
          ascending: '????????? ?? ???????? ???',
          descending: '????????? ?? ???????? ???',
          none: '??? ?????????',
          remove: '???????? ???????????'
        },
        column: {
          hide: '????? ????????'
        },
        aggregation: {
          count: '???? ??????: ',
          sum: '????: ',
          avg: '??????: ',
          min: '???-?????: ',
          max: '???-?????: '
        },
        pinning: {
          pinLeft: '???????? ?????',
          pinRight: '???????? ??????',
          unpin: '??????????'
        },
        columnMenu: {
          close: '???????'
        },
        gridMenu: {
          aria: {
            buttonLabel: '???? ?? ?????????'
          },
          columns: '??????:',
          importerTitle: '??????????? ?? ????',
          exporterAllAsCsv: '???????????? ?? ??????? ???? csv',
          exporterVisibleAsCsv: '???????????? ?? ???????? ????? ???? csv',
          exporterSelectedAsCsv: '???????????? ?? ????????? ????? ???? csv',
          exporterAllAsPdf: '???????????? ?? ??????? ???? pdf',
          exporterVisibleAsPdf: '???????????? ?? ???????? ????? ???? pdf',
          exporterSelectedAsPdf: '???????????? ?? ????????? ????? ???? pdf',
          clearAllFilters: '???????? ?????? ??????'
        },
        importer: {
          noHeaders: '??????? ?? ???????? ?? ?????? ?? ????? ?????????, ?????? ??? ?? ??????',
          noObjects: '???????? ?? ?????? ?? ????? ?????????, ?????? ??????? ?? ?????, ???????? ?? ??????',
          invalidCsv: '?????? ?? ???? ?? ???? ?????????, ??????? ??, ?? ? ??????? CSV ????',
          invalidJson: '?????? ?? ???? ?? ???? ?????????, ??????? ??, ?? ? ??????? JSON ????',
          jsonNotArray: '????????????? JSON ???? ?????? ?? ??????? ?????, ????????????.'
        },
        pagination: {
          aria: {
            pageToFirst: '??? ????? ????????',
            pageBack: '???????? ?????',
            pageSelected: '??????? ????????',
            pageForward: '???????? ??????',
            pageToLast: '??? ???????? ????????'
          },
          sizes: '?????? ?? ????????',
          totalItems: '??????',
          through: '??',
          of: '??'
        },
        grouping: {
          group: '?????????',
          ungroup: '?????????? ?? ???????????',
          aggregate_count: '????: ????',
          aggregate_sum: '????: ????',
          aggregate_max: '????: ????????',
          aggregate_min: '????: ???????',
          aggregate_avg: '????: ??????',
          aggregate_remove: '????: ??????????'
        },
        validate: {
          error: '??????:',
          minLength: '?????????? ?????? ?? ??????? ???? THRESHOLD ???????.',
          maxLength: '?????????? ?? ?????? ?? ??????? ?????? ?? THRESHOLD ???????.',
          required: '?????????? ? ????????.'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      var lang = {
              aggregate: {
                  label: 'položky'
              },
              groupPanel: {
                  description: 'Presunte záhlaví zde pro vytvorení skupiny dle sloupce.'
              },
              search: {
                  placeholder: 'Hledat...',
                  showingItems: 'Zobrazuji položky:',
                  selectedItems: 'Vybrané položky:',
                  totalItems: 'Celkem položek:',
                  size: 'Velikost strany:',
                  first: 'První strana',
                  next: 'Další strana',
                  previous: 'Predchozí strana',
                  last: 'Poslední strana'
              },
              menu: {
                  text: 'Vyberte sloupec:'
              },
              sort: {
                  ascending: 'Seradit od A-Z',
                  descending: 'Seradit od Z-A',
                  remove: 'Odebrat serazení'
              },
              column: {
                  hide: 'Schovat sloupec'
              },
              aggregation: {
                  count: 'celkem rádku: ',
                  sum: 'celkem: ',
                  avg: 'avg: ',
                  min: 'min.: ',
                  max: 'max.: '
              },
              pinning: {
                  pinLeft: 'Zamknout vlevo',
                  pinRight: 'Zamknout vpravo',
                  unpin: 'Odemknout'
              },
              gridMenu: {
                  columns: 'Sloupce:',
                  importerTitle: 'Importovat soubor',
                  exporterAllAsCsv: 'Exportovat všechna data do csv',
                  exporterVisibleAsCsv: 'Exportovat viditelná data do csv',
                  exporterSelectedAsCsv: 'Exportovat vybraná data do csv',
                  exporterAllAsPdf: 'Exportovat všechna data do pdf',
                  exporterVisibleAsPdf: 'Exportovat viditelná data do pdf',
                  exporterSelectedAsPdf: 'Exportovat vybraná data do pdf',
                  exporterAllAsExcel: 'Exportovat všechna data do excel',
                  exporterVisibleAsExcel: 'Exportovat viditelná data do excel',
                  exporterSelectedAsExcel: 'Exportovat vybraná data do excel',
                  clearAllFilters: 'Odstranit všechny filtry'
              },
              importer: {
                  noHeaders: 'Názvy sloupcu se nepodarilo získat, obsahuje soubor záhlaví?',
                  noObjects: 'Data se nepodarilo zpracovat, obsahuje soubor rádky mimo záhlaví?',
                  invalidCsv: 'Soubor nelze zpracovat, jedná se o CSV?',
                  invalidJson: 'Soubor nelze zpracovat, je to JSON?',
                  jsonNotArray: 'Soubor musí obsahovat json. Ukoncuji..'
              },
              pagination: {
                  sizes: 'položek na stránku',
                  totalItems: 'položek'
              },
              grouping: {
                  group: 'Seskupit',
                  ungroup: 'Odebrat seskupení',
                  aggregate_count: 'Agregace: Count',
                  aggregate_sum: 'Agregace: Sum',
                  aggregate_max: 'Agregace: Max',
                  aggregate_min: 'Agregace: Min',
                  aggregate_avg: 'Agregace: Avg',
                  aggregate_remove: 'Agregace: Odebrat'
              }
          };

          // support varianty of different czech keys.
          $delegate.add('cs', lang);
          $delegate.add('cz', lang);
          $delegate.add('cs-cz', lang);
          $delegate.add('cs-CZ', lang);
      return $delegate;
    }]);
  }]);
})();

(function() {
	angular.module('ui.grid').config(['$provide', function($provide) {
		$provide.decorator('i18nService', ['$delegate', function($delegate) {
			$delegate.add('da', {
				aggregate: {
					label: 'artikler'
				},
				groupPanel: {
					description: 'Grupér rækker udfra en kolonne ved at trække dens overskift hertil.'
				},
				search: {
					placeholder: 'Søg...',
					showingItems: 'Viste rækker:',
					selectedItems: 'Valgte rækker:',
					totalItems: 'Rækker totalt:',
					size: 'Side størrelse:',
					first: 'Første side',
					next: 'Næste side',
					previous: 'Forrige side',
					last: 'Sidste side'
				},
				menu: {
					text: 'Vælg kolonner:'
				},
				sort: {
					ascending: 'Sorter stigende',
					descending: 'Sorter faldende',
					none: 'Sorter ingen',
					remove: 'Fjern sortering'
				},
				column: {
					hide: 'Skjul kolonne'
				},
				aggregation: {
					count: 'antal rækker: ',
					sum: 'sum: ',
					avg: 'gns: ',
					min: 'min: ',
					max: 'max: '
				},
				pinning: {
					pinLeft: 'Fastgør til venstre',
					pinRight: 'Fastgør til højre',
					unpin: 'Frigør'
				},
				gridMenu: {
					columns: 'Kolonner:',
					importerTitle: 'Importer fil',
					exporterAllAsCsv: 'Eksporter alle data som csv',
					exporterVisibleAsCsv: 'Eksporter synlige data som csv',
					exporterSelectedAsCsv: 'Eksporter markerede data som csv',
					exporterAllAsPdf: 'Eksporter alle data som pdf',
					exporterVisibleAsPdf: 'Eksporter synlige data som pdf',
					exporterSelectedAsPdf: 'Eksporter markerede data som pdf',
					exporterAllAsExcel: 'Eksporter alle data som excel',
					exporterVisibleAsExcel: 'Eksporter synlige data som excel',
					exporterSelectedAsExcel: 'Eksporter markerede data som excel',
					clearAllFilters: 'Clear all filters'
				},
				importer: {
					noHeaders: 'Column names were unable to be derived, does the file have a header?',
					noObjects: 'Objects were not able to be derived, was there data in the file other than headers?',
					invalidCsv: 'File was unable to be processed, is it valid CSV?',
					invalidJson: 'File was unable to be processed, is it valid Json?',
					jsonNotArray: 'Imported json file must contain an array, aborting.'
				},
				pagination: {
					aria: {
						pageToFirst: 'Gå til første',
						pageBack: 'Gå tilbage',
						pageSelected: 'Valgte side',
						pageForward: 'Gå frem',
						pageToLast: 'Gå til sidste'
					},
					sizes: 'genstande per side',
					totalItems: 'genstande',
					through: 'gennem',
					of: 'af'
				}
			});
			return $delegate;
		}]);
	}]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function ($provide) {
    $provide.decorator('i18nService', ['$delegate', function ($delegate) {
      $delegate.add('de', {
        headerCell: {
          aria: {
            defaultFilterLabel: 'Filter für Spalte',
            removeFilter: 'Filter löschen',
            columnMenuButtonLabel: 'Spaltenmenü',
            column: 'Spalte'
          },
          priority: 'Priorität:',
          filterLabel: "Filter für Spalte: "
        },
        aggregate: {
          label: 'Eintrag'
        },
        groupPanel: {
          description: 'Ziehen Sie eine Spaltenüberschrift hierhin, um nach dieser Spalte zu gruppieren.'
        },
        search: {
          aria: {
            selected: 'Zeile markiert',
            notSelected: 'Zeile nicht markiert'
          },
          placeholder: 'Suche...',
          showingItems: 'Zeige Einträge:',
          selectedItems: 'Ausgewählte Einträge:',
          totalItems: 'Einträge gesamt:',
          size: 'Einträge pro Seite:',
          first: 'Erste Seite',
          next: 'Nächste Seite',
          previous: 'Vorherige Seite',
          last: 'Letzte Seite'
        },
        menu: {
          text: 'Spalten auswählen:'
        },
        sort: {
          ascending: 'aufsteigend sortieren',
          descending: 'absteigend sortieren',
          none: 'keine Sortierung',
          remove: 'Sortierung zurücksetzen'
        },
        column: {
          hide: 'Spalte ausblenden'
        },
        aggregation: {
          count: 'Zeilen insgesamt: ',
          sum: 'gesamt: ',
          avg: 'Durchschnitt: ',
          min: 'min: ',
          max: 'max: '
        },
        pinning: {
            pinLeft: 'Links anheften',
            pinRight: 'Rechts anheften',
            unpin: 'Lösen'
        },
        columnMenu: {
          close: 'Schließen'
        },
        gridMenu: {
          aria: {
            buttonLabel: 'Tabellenmenü'
          },
          columns: 'Spalten:',
          importerTitle: 'Datei importieren',
          exporterAllAsCsv: 'Alle Daten als CSV exportieren',
          exporterVisibleAsCsv: 'Sichtbare Daten als CSV exportieren',
          exporterSelectedAsCsv: 'Markierte Daten als CSV exportieren',
          exporterAllAsPdf: 'Alle Daten als PDF exportieren',
          exporterVisibleAsPdf: 'Sichtbare Daten als PDF exportieren',
          exporterSelectedAsPdf: 'Markierte Daten als PDF exportieren',
          exporterAllAsExcel: 'Alle Daten als Excel exportieren',
          exporterVisibleAsExcel: 'Sichtbare Daten als Excel exportieren',
          exporterSelectedAsExcel: 'Markierte Daten als Excel exportieren',
          clearAllFilters: 'Alle Filter zurücksetzen'
        },
        importer: {
          noHeaders: 'Es konnten keine Spaltennamen ermittelt werden. Sind in der Datei Spaltendefinitionen enthalten?',
          noObjects: 'Es konnten keine Zeileninformationen gelesen werden, Sind in der Datei außer den Spaltendefinitionen auch Daten enthalten?',
          invalidCsv: 'Die Datei konnte nicht eingelesen werden, ist es eine gültige CSV-Datei?',
          invalidJson: 'Die Datei konnte nicht eingelesen werden. Enthält sie gültiges JSON?',
          jsonNotArray: 'Die importierte JSON-Datei muß ein Array enthalten. Breche Import ab.'
        },
        pagination: {
          aria: {
            pageToFirst: 'Zum Anfang',
            pageBack: 'Seite zurück',
            pageSelected: 'Ausgewählte Seite',
            pageForward: 'Seite vor',
            pageToLast: 'Zum Ende'
          },
          sizes: 'Einträge pro Seite',
          totalItems: 'Einträgen',
          through: 'bis',
          of: 'von'
        },
        grouping: {
            group: 'Gruppieren',
            ungroup: 'Gruppierung aufheben',
            aggregate_count: 'Agg: Anzahl',
            aggregate_sum: 'Agg: Summe',
            aggregate_max: 'Agg: Maximum',
            aggregate_min: 'Agg: Minimum',
            aggregate_avg: 'Agg: Mittelwert',
            aggregate_remove: 'Aggregation entfernen'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('es-ct', {
        headerCell: {
          aria: {
            defaultFilterLabel: 'Filtre per columna',
            removeFilter: 'Elimina el filtre',
            columnMenuButtonLabel: 'Menú de Columna',
            column: 'Columna'
          },
          priority: 'Priority:',
          filterLabel: 'Filtre per columna: '
        },
        aggregate: {
          label: 'items'
        },
        groupPanel: {
          description: 'Arrossegueu una capçalera de columna aquí i deixeu-lo anar per agrupar per aquesta columna.'
        },
        search: {
          aria: {
            selected: 'Fila seleccionada',
            notSelected: 'Fila no seleccionada'
          },
          placeholder: 'Cerca...',
          showingItems: 'Ítems Mostrats:',
          selectedItems: 'Ítems Seleccionats:',
          totalItems: 'Ítems Totals:',
          size: 'Mida de la pàgina:',
          first: 'Primera Pàgina',
          next: 'Propera Pàgina',
          previous: 'Pàgina Anterior',
          last: 'Última Pàgina'
        },
        menu: {
          text: 'Triar Columnes:'
        },
        sort: {
          ascending: 'Ordena Ascendent',
          descending: 'Ordena Descendent',
          none: 'Sense Ordre',
          remove: 'Eliminar Ordre'
        },
        column: {
          hide: 'Amaga la Columna'
        },
        aggregation: {
          count: 'Files Totals: ',
          sum: 'total: ',
          avg: 'mitjà: ',
          min: 'mín: ',
          max: 'màx: '
        },
        pinning: {
          pinLeft: "Fixar a l'Esquerra",
          pinRight: 'Fixar a la Dreta',
          unpin: 'Treure Fixació'
        },
        columnMenu: {
          close: 'Tanca'
        },
        gridMenu: {
          aria: {
            buttonLabel: 'Menú de Quadrícula'
          },
          columns: 'Columnes:',
          importerTitle: 'Importa el fitxer',
          exporterAllAsCsv: 'Exporta tot com CSV',
          exporterVisibleAsCsv: 'Exporta les dades visibles com a CSV',
          exporterSelectedAsCsv: 'Exporta les dades seleccionades com a CSV',
          exporterAllAsPdf: 'Exporta tot com PDF',
          exporterVisibleAsPdf: 'Exporta les dades visibles com a PDF',
          exporterSelectedAsPdf: 'Exporta les dades seleccionades com a PDF',
          exporterAllAsExcel: 'Exporta tot com Excel',
          exporterVisibleAsExcel: 'Exporta les dades visibles com Excel',
          exporterSelectedAsExcel: 'Exporta les dades seleccionades com Excel',
          clearAllFilters: 'Netejar tots els filtres'
        },
        importer: {
          noHeaders: "No va ser possible derivar els noms de les columnes, té encapçalats l'arxiu?",
          noObjects: "No va ser possible obtenir registres, conté dades l'arxiu, a part de les capçaleres?",
          invalidCsv: "No va ser possible processar l'arxiu, ¿és un CSV vàlid?",
          invalidJson: "No va ser possible processar l'arxiu, ¿és un JSON vàlid?",
          jsonNotArray: 'El fitxer json importat ha de contenir una matriu, avortant.'
        },
        pagination: {
          aria: {
            pageToFirst: 'Page to first',
            pageBack: 'Page back',
            pageSelected: 'Selected page',
            pageForward: 'Page forward',
            pageToLast: 'Page to last'
          },
          sizes: 'ítems per pàgina',
          totalItems: 'ítems',
          through: 'a',
          of: 'de'
        },
        grouping: {
          group: 'Agrupar',
          ungroup: 'Desagrupar',
          aggregate_count: 'Agr: Compte',
          aggregate_sum: 'Agr: Sum',
          aggregate_max: 'Agr: Máx',
          aggregate_min: 'Agr: Mín',
          aggregate_avg: 'Agr: Mitjà',
          aggregate_remove: 'Agr: Treure'
        },
        validate: {
          error: 'Error:',
          minLength: 'El valor ha de tenir almenys caràcters THRESHOLD.',
          maxLength: 'El valor ha de tenir com a màxim caràcters THRESHOLD.',
          required: 'Un valor és necessari.'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('es', {
        aggregate: {
          label: 'Artículos'
        },
        groupPanel: {
          description: 'Arrastre un encabezado de columna aquí y suéltelo para agrupar por esa columna.'
        },
        search: {
          placeholder: 'Buscar...',
          showingItems: 'Artículos Mostrados:',
          selectedItems: 'Artículos Seleccionados:',
          totalItems: 'Artículos Totales:',
          size: 'Tamaño de Página:',
          first: 'Primera Página',
          next: 'Página Siguiente',
          previous: 'Página Anterior',
          last: 'Última Página'
        },
        menu: {
          text: 'Elegir columnas:'
        },
        sort: {
          ascending: 'Orden Ascendente',
          descending: 'Orden Descendente',
          remove: 'Sin Ordenar'
        },
        column: {
          hide: 'Ocultar la columna'
        },
        aggregation: {
          count: 'filas totales: ',
          sum: 'total: ',
          avg: 'media: ',
          min: 'min: ',
          max: 'max: '
        },
        pinning: {
          pinLeft: 'Fijar a la Izquierda',
          pinRight: 'Fijar a la Derecha',
          unpin: 'Quitar Fijación'
        },
        gridMenu: {
          columns: 'Columnas:',
          importerTitle: 'Importar archivo',
          exporterAllAsCsv: 'Exportar todo como csv',
          exporterVisibleAsCsv: 'Exportar vista como csv',
          exporterSelectedAsCsv: 'Exportar selección como csv',
          exporterAllAsPdf: 'Exportar todo como pdf',
          exporterVisibleAsPdf: 'Exportar vista como pdf',
          exporterSelectedAsPdf: 'Exportar selección como pdf',
          exporterAllAsExcel: 'Exportar todo como excel',
          exporterVisibleAsExcel: 'Exportar vista como excel',
          exporterSelectedAsExcel: 'Exportar selección como excel',
          clearAllFilters: 'Limpiar todos los filtros'
        },
        importer: {
          noHeaders: 'No fue posible derivar los nombres de las columnas, ¿tiene encabezados el archivo?',
          noObjects: 'No fue posible obtener registros, ¿contiene datos el archivo, aparte de los encabezados?',
          invalidCsv: 'No fue posible procesar el archivo, ¿es un CSV válido?',
          invalidJson: 'No fue posible procesar el archivo, ¿es un Json válido?',
          jsonNotArray: 'El archivo json importado debe contener un array, abortando.'
        },
        pagination: {
          aria: {
										pageToFirst: 'Inicio',
										pageBack: 'Anterior',
										pageSelected: 'Seleccionada',
										pageForward: 'Siguiente',
										pageToLast: 'Fin'
									},
          through: 'mediante',
          sizes: 'Grupos de registros',
          totalItems: 'registros',
          of: 'de'
        },
        grouping: {
          group: 'Agrupar',
          ungroup: 'Desagrupar',
          aggregate_count: 'Agr: Cont',
          aggregate_sum: 'Agr: Sum',
          aggregate_max: 'Agr: Máx',
          aggregate_min: 'Agr: Min',
          aggregate_avg: 'Agr: Prom',
          aggregate_remove: 'Agr: Quitar'
        }
      });
      return $delegate;
    }]);
}]);
})();

/**
 * Translated by: R. Salarmehr
 *                M. Hosseynzade
 *                Using Vajje.com online dictionary.
 */
(function () {
  angular.module('ui.grid').config(['$provide', function ($provide) {
    $provide.decorator('i18nService', ['$delegate', function ($delegate) {
      $delegate.add('fa', {
        aggregate: {
          label: '???'
        },
        groupPanel: {
          description: '????? ?? ???? ?? ???? ? ?? ????? ?? ?? ???? ??? ??.'
        },
        search: {
          placeholder: '?????...',
          showingItems: '????? ?????:',
          selectedItems: '???\u200c??? ?????? ???:',
          totalItems: '????? ?????:',
          size: '??????\u200c? ????:',
          first: '????? ????',
          next: '????\u200c?\u200c????',
          previous: '????\u200c?\u200c ????',
          last: '????? ????'
        },
        menu: {
          text: '????\u200c??? ???????:'
        },
        sort: {
          ascending: '????? ?????',
          descending: '????? ?????',
          remove: '??? ???? ????'
        },
        column: {
          hide: '?????\u200c???? ????'
        },
        aggregation: {
          count: '?????: ',
          sum: '?????: ',
          avg: '???????: ',
          min: '??????: ',
          max: '???????: '
        },
        pinning: {
          pinLeft: '??? ???? ??? ??',
          pinRight: '??? ???? ??? ????',
          unpin: '??? ???'
        },
        gridMenu: {
          columns: '????\u200c??:',
          importerTitle: '???? ???? ????',
          exporterAllAsCsv: '????? ???? ????\u200c?? ?? ???? csv',
          exporterVisibleAsCsv: '????? ????\u200c??? ???? ?????? ?? ???? csv',
          exporterSelectedAsCsv: '????? ????\u200c??? ??????\u200c??? ?? ???? csv',
          exporterAllAsPdf: '????? ???? ????\u200c?? ?? ???? pdf',
          exporterVisibleAsPdf: '????? ????\u200c??? ???? ?????? ?? ???? pdf',
          exporterSelectedAsPdf: '????? ????\u200c??? ??????\u200c??? ?? ???? pdf',
          clearAllFilters: '??? ???? ???? ?????'
        },
        importer: {
          noHeaders: '??? ???? ???? ??????? ????. ??? ???? ????? ?????',
          noObjects: '???? ???? ??????? ??????. ??? ?? ?? ?????\u200c?? ?? ???? ???? ???? ?????',
          invalidCsv: '???? ???? ?????? ????. ??? ????  csv  ????? ????',
          invalidJson: '???? ???? ?????? ????. ??? ???? json   ????? ????',
          jsonNotArray: '???? json ???? ??? ???? ???? ????? ????. ?????? ???? ??.'
        },
        pagination: {
          sizes: '????? ?? ?? ????',
          totalItems: '?????',
          of: '??'
        },
        grouping: {
          group: '????\u200c????',
          ungroup: '??? ????\u200c????',
          aggregate_count: 'Agg: ?????',
          aggregate_sum: 'Agg: ???',
          aggregate_max: 'Agg: ??????',
          aggregate_min: 'Agg: ?????',
          aggregate_avg: 'Agg: ???????',
          aggregate_remove: 'Agg: ???'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('fi', {
        aggregate: {
          label: 'rivit'
        },
        groupPanel: {
          description: 'Raahaa ja pudota otsikko tähän ryhmittääksesi sarakkeen mukaan.'
        },
        search: {
          placeholder: 'Hae...',
          showingItems: 'Näytetään rivejä:',
          selectedItems: 'Valitut rivit:',
          totalItems: 'Rivejä yht.:',
          size: 'Näytä:',
          first: 'Ensimmäinen sivu',
          next: 'Seuraava sivu',
          previous: 'Edellinen sivu',
          last: 'Viimeinen sivu'
        },
        menu: {
          text: 'Valitse sarakkeet:'
        },
        sort: {
          ascending: 'Järjestä nouseva',
          descending: 'Järjestä laskeva',
          remove: 'Poista järjestys'
        },
        column: {
          hide: 'Piilota sarake'
        },
        aggregation: {
          count: 'Rivejä yht.: ',
          sum: 'Summa: ',
          avg: 'K.a.: ',
          min: 'Min: ',
          max: 'Max: '
        },
        pinning: {
         pinLeft: 'Lukitse vasemmalle',
          pinRight: 'Lukitse oikealle',
          unpin: 'Poista lukitus'
        },
        gridMenu: {
          columns: 'Sarakkeet:',
          importerTitle: 'Tuo tiedosto',
          exporterAllAsCsv: 'Vie tiedot csv-muodossa',
          exporterVisibleAsCsv: 'Vie näkyvä tieto csv-muodossa',
          exporterSelectedAsCsv: 'Vie valittu tieto csv-muodossa',
          exporterAllAsPdf: 'Vie tiedot pdf-muodossa',
          exporterVisibleAsPdf: 'Vie näkyvä tieto pdf-muodossa',
          exporterSelectedAsPdf: 'Vie valittu tieto pdf-muodossa',
          exporterAllAsExcel: 'Vie tiedot excel-muodossa',
          exporterVisibleAsExcel: 'Vie näkyvä tieto excel-muodossa',
          exporterSelectedAsExcel: 'Vie valittu tieto excel-muodossa',
          clearAllFilters: 'Puhdista kaikki suodattimet'
        },
        importer: {
          noHeaders: 'Sarakkeen nimiä ei voitu päätellä, onko tiedostossa otsikkoriviä?',
          noObjects: 'Tietoja ei voitu lukea, onko tiedostossa muuta kuin otsikkot?',
          invalidCsv: 'Tiedostoa ei voitu käsitellä, oliko se CSV-muodossa?',
          invalidJson: 'Tiedostoa ei voitu käsitellä, oliko se JSON-muodossa?',
          jsonNotArray: 'Tiedosto ei sisältänyt taulukkoa, lopetetaan.'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('fr', {
        headerCell: {
          aria: {
            defaultFilterLabel: 'Filtre de la colonne',
            removeFilter: 'Supprimer le filtre',
            columnMenuButtonLabel: 'Menu de la colonne'
          },
          priority: 'Priorité:',
          filterLabel: "Filtre de la colonne: "
        },
        aggregate: {
          label: 'éléments'
        },
        groupPanel: {
          description: 'Faites glisser une en-tête de colonne ici pour créer un groupe de colonnes.'
        },
        search: {
          placeholder: 'Recherche...',
          showingItems: 'Affichage des éléments :',
          selectedItems: 'Éléments sélectionnés :',
          totalItems: 'Nombre total d\'éléments:',
          size: 'Taille de page:',
          first: 'Première page',
          next: 'Page Suivante',
          previous: 'Page précédente',
          last: 'Dernière page'
        },
        menu: {
          text: 'Choisir des colonnes :'
        },
        sort: {
          ascending: 'Trier par ordre croissant',
          descending: 'Trier par ordre décroissant',
          none: 'Aucun tri',
          remove: 'Enlever le tri'
        },
        column: {
          hide: 'Cacher la colonne'
        },
        aggregation: {
          count: 'lignes totales: ',
          sum: 'total: ',
          avg: 'moy: ',
          min: 'min: ',
          max: 'max: '
        },
        pinning: {
          pinLeft: 'Épingler à gauche',
          pinRight: 'Épingler à droite',
          unpin: 'Détacher'
        },
        columnMenu: {
          close: 'Fermer'
        },
        gridMenu: {
          aria: {
            buttonLabel: 'Menu du tableau'
          },
          columns: 'Colonnes:',
          importerTitle: 'Importer un fichier',
          exporterAllAsCsv: 'Exporter toutes les données en CSV',
          exporterVisibleAsCsv: 'Exporter les données visibles en CSV',
          exporterSelectedAsCsv: 'Exporter les données sélectionnées en CSV',
          exporterAllAsPdf: 'Exporter toutes les données en PDF',
          exporterVisibleAsPdf: 'Exporter les données visibles en PDF',
          exporterSelectedAsPdf: 'Exporter les données sélectionnées en PDF',
          exporterAllAsExcel: 'Exporter toutes les données en Excel',
          exporterVisibleAsExcel: 'Exporter les données visibles en Excel',
          exporterSelectedAsExcel: 'Exporter les données sélectionnées en Excel',
          clearAllFilters: 'Nettoyez tous les filtres'
        },
        importer: {
          noHeaders: 'Impossible de déterminer le nom des colonnes, le fichier possède-t-il une en-tête ?',
          noObjects: 'Aucun objet trouvé, le fichier possède-t-il des données autres que l\'en-tête ?',
          invalidCsv: 'Le fichier n\'a pas pu être traité, le CSV est-il valide ?',
          invalidJson: 'Le fichier n\'a pas pu être traité, le JSON est-il valide ?',
          jsonNotArray: 'Le fichier JSON importé doit contenir un tableau, abandon.'
        },
        pagination: {
          aria: {
            pageToFirst: 'Aller à la première page',
            pageBack: 'Page précédente',
            pageSelected: 'Page sélectionnée',
            pageForward: 'Page suivante',
            pageToLast: 'Aller à la dernière page'
          },
          sizes: 'éléments par page',
          totalItems: 'éléments',
          through: 'à',
          of: 'sur'
        },
        grouping: {
          group: 'Grouper',
          ungroup: 'Dégrouper',
          aggregate_count: 'Agg: Compter',
          aggregate_sum: 'Agg: Somme',
          aggregate_max: 'Agg: Max',
          aggregate_min: 'Agg: Min',
          aggregate_avg: 'Agg: Moy',
          aggregate_remove: 'Agg: Retirer'
        },
        validate: {
          error: 'Erreur:',
          minLength: 'La valeur doit être supérieure ou égale à THRESHOLD caractères.',
          maxLength: 'La valeur doit être inférieure ou égale à THRESHOLD caractères.',
          required: 'Une valeur est nécéssaire.'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function ($provide) {
    $provide.decorator('i18nService', ['$delegate', function ($delegate) {
      $delegate.add('he', {
        aggregate: {
          label: 'items'
        },
        groupPanel: {
          description: '???? ????? ???? ????? ???? ???? ????? ??.'
        },
        search: {
          placeholder: '???...',
          showingItems: '????:',
          selectedItems: '??"? ?????:',
          totalItems: '??"? ??????:',
          size: '?????? ???:',
          first: '?? ?????',
          next: '?? ???',
          previous: '?? ????',
          last: '?? ?????'
        },
        menu: {
          text: '??? ??????:'
        },
        sort: {
          ascending: '??? ????',
          descending: '??? ????',
          remove: '???'
        },
        column: {
          hide: '??? ????'
        },
        aggregation: {
          count: 'total rows: ',
          sum: 'total: ',
          avg: 'avg: ',
          min: 'min: ',
          max: 'max: '
        },
        gridMenu: {
          columns: 'Columns:',
          importerTitle: 'Import file',
          exporterAllAsCsv: 'Export all data as csv',
          exporterVisibleAsCsv: 'Export visible data as csv',
          exporterSelectedAsCsv: 'Export selected data as csv',
          exporterAllAsPdf: 'Export all data as pdf',
          exporterVisibleAsPdf: 'Export visible data as pdf',
          exporterSelectedAsPdf: 'Export selected data as pdf',
          exporterAllAsExcel: 'Export all data as excel',
          exporterVisibleAsExcel: 'Export visible data as excel',
          exporterSelectedAsExcel: 'Export selected data as excel',
          clearAllFilters: 'Clean all filters'
        },
        importer: {
          noHeaders: 'Column names were unable to be derived, does the file have a header?',
          noObjects: 'Objects were not able to be derived, was there data in the file other than headers?',
          invalidCsv: 'File was unable to be processed, is it valid CSV?',
          invalidJson: 'File was unable to be processed, is it valid Json?',
          jsonNotArray: 'Imported json file must contain an array, aborting.'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('hy', {
        aggregate: {
          label: '????????'
        },
        groupPanel: {
          description: '??? ???? ??????????? ????? ????? ? ???? ????????? ??????:'
        },
        search: {
          placeholder: '???????...',
          showingItems: '?????????? ?????????',
          selectedItems: '???????:',
          totalItems: '?????????',
          size: '?????? ?????? ??????',
          first: '?????? ??',
          next: '?????? ??',
          previous: '?????? ??',
          last: '?????? ??'
        },
        menu: {
          text: '?????? ????????:'
        },
        sort: {
          ascending: '????? ??????',
          descending: '??????? ??????',
          remove: '????? '
        },
        column: {
          hide: '??????? ??????'
        },
        aggregation: {
          count: '???????? ???? ',
          sum: '????????? ',
          avg: '?????? ',
          min: '???? ',
          max: '????? '
        },
        pinning: {
          pinLeft: '?????? ??? ???????',
          pinRight: '?????? ?? ???????',
          unpin: '???????'
        },
        gridMenu: {
          columns: '???????:',
          importerTitle: '????????? ????',
          exporterAllAsCsv: '????????? ??????? CSV',
          exporterVisibleAsCsv: '????????? ??????? ????????? CSV',
          exporterSelectedAsCsv: '????????? ??????? ????????? CSV',
          exporterAllAsPdf: '????????? PDF',
          exporterVisibleAsPdf: '????????? ??????? ????????? PDF',
          exporterSelectedAsPdf: '????????? ??????? ????????? PDF',
          exporterAllAsExcel: '????????? excel',
          exporterVisibleAsExcel: '????????? ??????? ????????? excel',
          exporterSelectedAsExcel: '????????? ??????? ????????? excel',
          clearAllFilters: '?????? ????? ????????'
        },
        importer: {
          noHeaders: '???????? ????? ?????? ???? ??????????: ??????? ????? ???? ?????????:',
          noObjects: '???????? ????? ?????? ?????????: ??????? ??????? ??? ????????:',
          invalidCsv: '???????? ????? ?????? ?????: ??????? ??? ????? CSV ?:',
          invalidJson: '???????? ????? ?????? ?????: ??????? ??? ????? Json ?:',
          jsonNotArray: '?????????? json ????? ???? ? ????????? ???????, ????????? ?:'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('is', {
        headerCell: {
          aria: {
            defaultFilterLabel: 'Sía fyrir dálk',
            removeFilter: 'Fjarlægja síu',
            columnMenuButtonLabel: 'Dálkavalmynd'
          },
          priority: 'Forgangsröðun:',
          filterLabel: "Sía fyrir dálka: "
        },
        aggregate: {
          label: 'hlutir'
        },
        groupPanel: {
          description: 'Dragðu dálkhaus hingað til að flokka saman eftir þeim dálki.'
        },
        search: {
          placeholder: 'Leita...',
          showingItems: 'Sýni hluti:',
          selectedItems: 'Valdir hlutir:',
          totalItems: 'Hlutir alls:',
          size: 'Stærð síðu:',
          first: 'Fyrsta síða',
          next: 'Næsta síða',
          previous: 'Fyrri síða',
          last: 'Síðasta síða'
        },
        menu: {
          text: 'Veldu dálka:'
        },
        sort: {
          ascending: 'Raða hækkandi',
          descending: 'Raða lækkandi',
          none: 'Engin röðun',
          remove: 'Fjarlægja röðun'
        },
        column: {
          hide: 'Fela dálk'
        },
        aggregation: {
          count: 'fjöldi raða: ',
          sum: 'summa: ',
          avg: 'meðaltal: ',
          min: 'lágmark: ',
          max: 'hámark: '
        },
        pinning: {
          pinLeft: 'Festa til vinstri',
          pinRight: 'Festa til hægri',
          unpin: 'Losa'
        },
        columnMenu: {
          close: 'Loka'
        },
        gridMenu: {
          aria: {
            buttonLabel: 'Töflu valmynd'
          },
          columns: 'Dálkar:',
          importerTitle: 'Flytja inn skjal',
          exporterAllAsCsv: 'Flytja út gögn sem csv',
          exporterVisibleAsCsv: 'Flytja út sýnileg gögn sem csv',
          exporterSelectedAsCsv: 'Flytja út valin gögn sem csv',
          exporterAllAsPdf: 'Flytja út öll gögn sem pdf',
          exporterVisibleAsPdf: 'Flytja út sýnileg gögn sem pdf',
          exporterSelectedAsPdf: 'Flytja út valin gögn sem pdf',
          clearAllFilters: 'Hreinsa allar síur'
        },
        importer: {
          noHeaders: 'Ekki hægt að vinna dálkanöfn úr skjalinu, er skjalið örugglega með haus?',
          noObjects: 'Ekki hægt að vinna hluti úr skjalinu, voru örugglega gögn í skjalinu önnur en hausinn?',
          invalidCsv: 'Tókst ekki að vinna skjal, er það örggulega gilt CSV?',
          invalidJson: 'Tókst ekki að vinna skjal, er það örugglega gilt Json?',
          jsonNotArray: 'Innflutt json skjal verður að innihalda fylki, hætti við.'
        },
        pagination: {
          aria: {
            pageToFirst: 'Fletta að fyrstu',
            pageBack: 'Fletta til baka',
            pageSelected: 'Valin síða',
            pageForward: 'Fletta áfram',
            pageToLast: 'Fletta að síðustu'
          },
          sizes: 'hlutir á síðu',
          totalItems: 'hlutir',
          through: 'gegnum',
          of: 'af'
        },
        grouping: {
          group: 'Flokka',
          ungroup: 'Sundurliða',
          aggregate_count: 'Fjöldi: ',
          aggregate_sum: 'Summa: ',
          aggregate_max: 'Hámark: ',
          aggregate_min: 'Lágmark: ',
          aggregate_avg: 'Meðaltal: ',
          aggregate_remove: 'Fjarlægja: '
        },
        validate: {
          error: 'Villa:',
          minLength: 'Gildi ætti að vera a.m.k. THRESHOLD stafa langt.',
          maxLength: 'Gildi ætti að vera í mesta lagi THRESHOLD stafa langt.',
          required: 'Þarf að hafa gildi.'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('it', {
        aggregate: {
          label: 'elementi'
        },
        groupPanel: {
          description: 'Trascina un\'intestazione all\'interno del gruppo della colonna.'
        },
        search: {
          placeholder: 'Ricerca...',
          showingItems: 'Mostra:',
          selectedItems: 'Selezionati:',
          totalItems: 'Totali:',
          size: 'Tot Pagine:',
          first: 'Prima',
          next: 'Prossima',
          previous: 'Precedente',
          last: 'Ultima'
        },
        menu: {
          text: 'Scegli le colonne:'
        },
        sort: {
          ascending: 'Asc.',
          descending: 'Desc.',
          remove: 'Annulla ordinamento'
        },
        column: {
          hide: 'Nascondi'
        },
        aggregation: {
          count: 'righe totali: ',
          sum: 'tot: ',
          avg: 'media: ',
          min: 'minimo: ',
          max: 'massimo: '
        },
        pinning: {
         pinLeft: 'Blocca a sx',
          pinRight: 'Blocca a dx',
          unpin: 'Blocca in alto'
        },
        gridMenu: {
          columns: 'Colonne:',
          importerTitle: 'Importa',
          exporterAllAsCsv: 'Esporta tutti i dati in CSV',
          exporterVisibleAsCsv: 'Esporta i dati visibili in CSV',
          exporterSelectedAsCsv: 'Esporta i dati selezionati in CSV',
          exporterAllAsPdf: 'Esporta tutti i dati in PDF',
          exporterVisibleAsPdf: 'Esporta i dati visibili in PDF',
          exporterSelectedAsPdf: 'Esporta i dati selezionati in PDF',
          exporterAllAsExcel: 'Esporta tutti i dati in excel',
          exporterVisibleAsExcel: 'Esporta i dati visibili in excel',
          exporterSelectedAsExcel: 'Esporta i dati selezionati in excel',
          clearAllFilters: 'Pulire tutti i filtri'
        },
        importer: {
          noHeaders: 'Impossibile reperire i nomi delle colonne, sicuro che siano indicati all\'interno del file?',
          noObjects: 'Impossibile reperire gli oggetti, sicuro che siano indicati all\'interno del file?',
          invalidCsv: 'Impossibile elaborare il file, sicuro che sia un CSV?',
          invalidJson: 'Impossibile elaborare il file, sicuro che sia un JSON valido?',
          jsonNotArray: 'Errore! Il file JSON da importare deve contenere un array.'
        },
        pagination: {
          aria: {
            pageToFirst: 'Prima',
            pageBack: 'Indietro',
            pageSelected: 'Pagina selezionata',
            pageForward: 'Avanti',
            pageToLast: 'Ultima'
          },
          sizes: 'elementi per pagina',
          totalItems: 'elementi',
          through: 'a',
          of: 'di'
        },
        grouping: {
          group: 'Raggruppa',
          ungroup: 'Separa',
          aggregate_count: 'Agg: N. Elem.',
          aggregate_sum: 'Agg: Somma',
          aggregate_max: 'Agg: Massimo',
          aggregate_min: 'Agg: Minimo',
          aggregate_avg: 'Agg: Media',
          aggregate_remove: 'Agg: Rimuovi'
        },
        validate: {
          error: 'Errore:',
          minLength: 'Lunghezza minima pari a THRESHOLD caratteri.',
          maxLength: 'Lunghezza massima pari a THRESHOLD caratteri.',
          required: 'Necessario inserire un valore.'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function() {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('ja', {
        headerCell: {
          aria: {
            defaultFilterLabel: '???????',
            removeFilter: '????????',
            columnMenuButtonLabel: '??????'
          },
          priority: '???:',
          filterLabel: "??????: "
        },
        aggregate: {
          label: '??'
        },
        groupPanel: {
          description: '???????????????????????????????????'
        },
        search: {
          placeholder: '??...',
          showingItems: '??????:',
          selectedItems: '??????:',
          totalItems: '?????:',
          size: '??????:',
          first: '??????',
          next: '?????',
          previous: '?????',
          last: '?????'
        },
        menu: {
          text: '????:'
        },
        sort: {
          ascending: '???????',
          descending: '???????',
          none: '??????',
          remove: '???????'
        },
        column: {
          hide: '?????'
        },
        aggregation: {
          count: '??: ',
          sum: '??: ',
          avg: '??: ',
          min: '??: ',
          max: '??: '
        },
        pinning: {
          pinLeft: '????',
          pinRight: '????',
          unpin: '????'
        },
        columnMenu: {
          close: '???'
        },
        gridMenu: {
          aria: {
            buttonLabel: '????????'
          },
          columns: '????/???:',
          importerTitle: '??????????',
          exporterAllAsCsv: '????????CSV?????????',
          exporterVisibleAsCsv: '????????CSV?????????',
          exporterSelectedAsCsv: '????????CSV?????????',
          exporterAllAsPdf: '????????PDF?????????',
          exporterVisibleAsPdf: '????????PDF?????????',
          exporterSelectedAsPdf: '????????PDF?????????',
          clearAllFilters: '????????????'
        },
        importer: {
          noHeaders: '??????????????????????????????????????',
          noObjects: '????????????????????????????????????????????????',
          invalidCsv: '?????????????????????CSV?????????????????',
          invalidJson: '?????????????????????JSON?????????????????',
          jsonNotArray: '???????JSON????????????????????????????????'
        },
        pagination: {
          aria: {
            pageToFirst: '??????',
            pageBack: '?????',
            pageSelected: '??????',
            pageForward: '?????',
            pageToLast: '??????'
          },
          sizes: '?/???',
          totalItems: '?',
          through: '??',
          of: '?/?'
        },
        grouping: {
          group: '?????',
          ungroup: '????????',
          aggregate_count: '????: ??',
          aggregate_sum: '????: ??',
          aggregate_max: '????: ??',
          aggregate_min: '????: ??',
          aggregate_avg: '????: ??',
          aggregate_remove: '????: ??'
        },
        validate: {
          error: 'Error:',
          minLength: 'THRESHOLD ??????????????',
          maxLength: 'THRESHOLD ??????????????',
          required: '???????'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('ko', {
        aggregate: {
          label: '???'
        },
        groupPanel: {
          description: '???? ????? ???? ?? ??? ?? ???? ???.'
        },
        search: {
          placeholder: '??...',
          showingItems: '?? ????:',
          selectedItems: '?? ??:',
          totalItems: '?? ??:',
          size: '??? ??:',
          first: '??? ???',
          next: '?? ???',
          previous: '?? ???',
          last: '??? ???'
        },
        menu: {
          text: '??? ?????:'
        },
        sort: {
          ascending: '???? ??',
          descending: '???? ??',
          remove: '?? ??'
        },
        column: {
          hide: '?? ??'
        },
        aggregation: {
          count: '?? ??: ',
          sum: '??: ',
          avg: '??: ',
          min: '??: ',
          max: '??: '
        },
        pinning: {
         pinLeft: '?? ?',
          pinRight: '??? ?',
          unpin: '? ??'
        },
        gridMenu: {
          columns: '??:',
          importerTitle: '?? ????',
          exporterAllAsCsv: 'csv? ?? ??? ????',
          exporterVisibleAsCsv: 'csv? ??? ??? ????',
          exporterSelectedAsCsv: 'csv? ??? ??? ????',
          exporterAllAsPdf: 'pdf? ?? ??? ????',
          exporterVisibleAsPdf: 'pdf? ??? ??? ????',
          exporterSelectedAsPdf: 'pdf? ?? ??? ????',
          clearAllFilters: '?? ??? ??'
        },
        importer: {
          noHeaders: '???? ???? ?? ????. ??? ??? ???? ??? ??? ???.',
          noObjects: '???? ???? ?? ????. ???? ??? ??? ??? ???.',
          invalidCsv: '??? ??? ? ????. ??? csv?? ??? ???.',
          invalidJson: '??? ??? ? ????. ??? json?? ??? ???.',
          jsonNotArray: 'json ??? ??? ???? ???.'
        },
        pagination: {
          sizes: '???? ??',
          totalItems: '?? ??'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('nl', {
        aggregate: {
          label: 'items'
        },
        groupPanel: {
          description: 'Sleep hier een kolomnaam heen om op te groeperen.'
        },
        search: {
          placeholder: 'Zoeken...',
          showingItems: 'Getoonde items:',
          selectedItems: 'Geselecteerde items:',
          totalItems: 'Totaal aantal items:',
          size: 'Items per pagina:',
          first: 'Eerste pagina',
          next: 'Volgende pagina',
          previous: 'Vorige pagina',
          last: 'Laatste pagina'
        },
        menu: {
          text: 'Kies kolommen:'
        },
        sort: {
          ascending: 'Sorteer oplopend',
          descending: 'Sorteer aflopend',
          remove: 'Verwijder sortering'
        },
        column: {
          hide: 'Verberg kolom'
        },
        aggregation: {
          count: 'Aantal rijen: ',
          sum: 'Som: ',
          avg: 'Gemiddelde: ',
          min: 'Min: ',
          max: 'Max: '
        },
        pinning: {
          pinLeft: 'Zet links vast',
          pinRight: 'Zet rechts vast',
          unpin: 'Maak los'
        },
        gridMenu: {
          columns: 'Kolommen:',
          importerTitle: 'Importeer bestand',
          exporterAllAsCsv: 'Exporteer alle data als csv',
          exporterVisibleAsCsv: 'Exporteer zichtbare data als csv',
          exporterSelectedAsCsv: 'Exporteer geselecteerde data als csv',
          exporterAllAsPdf: 'Exporteer alle data als pdf',
          exporterVisibleAsPdf: 'Exporteer zichtbare data als pdf',
          exporterSelectedAsPdf: 'Exporteer geselecteerde data als pdf',
          exporterAllAsExcel: 'Exporteer alle data als excel',
          exporterVisibleAsExcel: 'Exporteer zichtbare data als excel',
          exporterSelectedAsExcel: 'Exporteer alle data als excel',
          clearAllFilters: 'Alle filters wissen'
        },
        importer: {
          noHeaders: 'Kolomnamen kunnen niet worden afgeleid. Heeft het bestand een header?',
          noObjects: 'Objecten kunnen niet worden afgeleid. Bevat het bestand data naast de headers?',
          invalidCsv: 'Het bestand kan niet verwerkt worden. Is het een valide csv bestand?',
          invalidJson: 'Het bestand kan niet verwerkt worden. Is het valide json?',
          jsonNotArray: 'Het json bestand moet een array bevatten. De actie wordt geannuleerd.'
        },
        pagination: {
            sizes: 'items per pagina',
            totalItems: 'items',
            of: 'van de'
        },
        grouping: {
            group: 'Groepeer',
            ungroup: 'Groepering opheffen',
            aggregate_count: 'Agg: Aantal',
            aggregate_sum: 'Agg: Som',
            aggregate_max: 'Agg: Max',
            aggregate_min: 'Agg: Min',
            aggregate_avg: 'Agg: Gem',
            aggregate_remove: 'Agg: Verwijder'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('no', {
        headerCell: {
          aria: {
            defaultFilterLabel: 'Filter for kolonne',
            removeFilter: 'Fjern filter',
            columnMenuButtonLabel: 'Kolonnemeny'
          },
          priority: 'Prioritet:',
          filterLabel: "Filter for kolonne: "
        },
        aggregate: {
          label: 'elementer'
        },
        groupPanel: {
          description: 'Trekk en kolonneoverskrift hit og slipp den for å gruppere etter den kolonnen.'
        },
        search: {
          placeholder: 'Søk...',
          showingItems: 'Viste elementer:',
          selectedItems: 'Valgte elementer:',
          totalItems: 'Antall elementer:',
          size: 'Sidestørrelse:',
          first: 'Første side',
          next: 'Neste side',
          previous: 'Forrige side',
          last: 'Siste side'
        },
        menu: {
          text: 'Velg kolonner:'
        },
        sort: {
          ascending: 'Sortere stigende',
          descending: 'Sortere fallende',
          none: 'Ingen sortering',
          remove: 'Fjern sortering'
        },
        column: {
          hide: 'Skjul kolonne'
        },
        aggregation: {
          count: 'antall rader: ',
          sum: 'total: ',
          avg: 'gjennomsnitt: ',
          min: 'minimum: ',
          max: 'maksimum: '
        },
        pinning: {
          pinLeft: 'Fest til venstre',
          pinRight: 'Fest til høyre',
          unpin: 'Løsne'
        },
        columnMenu: {
          close: 'Lukk'
        },
        gridMenu: {
          aria: {
            buttonLabel: 'Grid Menu'
          },
          columns: 'Kolonner:',
          importerTitle: 'Importer fil',
          exporterAllAsCsv: 'Eksporter alle data som csv',
          exporterVisibleAsCsv: 'Eksporter synlige data som csv',
          exporterSelectedAsCsv: 'Eksporter utvalgte data som csv',
          exporterAllAsPdf: 'Eksporter alle data som pdf',
          exporterVisibleAsPdf: 'Eksporter synlige data som pdf',
          exporterSelectedAsPdf: 'Eksporter utvalgte data som pdf',
          exporterAllAsExcel: 'Eksporter alle data som excel',
          exporterVisibleAsExcel: 'Eksporter synlige data som excel',
          exporterSelectedAsExcel: 'Eksporter utvalgte data som excel',
          clearAllFilters: 'Clear all filters'
        },
        importer: {
          noHeaders: 'Kolonnenavn kunne ikke avledes. Har filen en overskrift?',
          noObjects: 'Objekter kunne ikke avledes. Er der andre data i filen enn overskriften?',
          invalidCsv: 'Filen kunne ikke behandles. Er den gyldig CSV?',
          invalidJson: 'Filen kunne ikke behandles. Er den gyldig JSON?',
          jsonNotArray: 'Importert JSON-fil må inneholde en liste. Avbryter.'
        },
        pagination: {
          aria: {
            pageToFirst: 'Gå til første side',
            pageBack: 'Gå til forrige side',
            pageSelected: 'Valgte side',
            pageForward: 'Gå til neste side',
            pageToLast: 'Gå til siste side'
          },
          sizes: 'elementer per side',
          totalItems: 'elementer',
          through: 'til',
          of: 'av'
        },
        grouping: {
          group: 'Gruppere',
          ungroup: 'Fjerne gruppering',
          aggregate_count: 'Agr: Antall',
          aggregate_sum: 'Agr: Sum',
          aggregate_max: 'Agr: Maksimum',
          aggregate_min: 'Agr: Minimum',
          aggregate_avg: 'Agr: Gjennomsnitt',
          aggregate_remove: 'Agr: Fjern'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('pl', {
        headerCell: {
          aria: {
            defaultFilterLabel: 'Filtr dla kolumny',
            removeFilter: 'Usun filtr',
            columnMenuButtonLabel: 'Opcje kolumny',
            column: 'Kolumna'
          },
          priority: 'Priorytet:',
          filterLabel: "Filtr dla kolumny: "
        },
        aggregate: {
          label: 'pozycji'
        },
        groupPanel: {
          description: 'Przeciagnij naglówek kolumny tutaj, aby pogrupowac wedlug niej.'
        },
        search: {
          aria: {
            selected: 'Wiersz zaznaczony',
            notSelected: 'Wiersz niezaznaczony'
          },
          placeholder: 'Szukaj...',
          showingItems: 'Widoczne pozycje:',
          selectedItems: 'Zaznaczone pozycje:',
          totalItems: 'Wszystkich pozycji:',
          size: 'Rozmiar strony:',
          first: 'Pierwsza strona',
          next: 'Nastepna strona',
          previous: 'Poprzednia strona',
          last: 'Ostatnia strona'
        },
        menu: {
          text: 'Wybierz kolumny:'
        },
        sort: {
          ascending: 'Sortuj rosnaco',
          descending: 'Sortuj malejaco',
          none: 'Brak sortowania',
          remove: 'Wylacz sortowanie'
        },
        column: {
          hide: 'Ukryj kolumne'
        },
        aggregation: {
          count: 'Razem pozycji: ',
            sum: 'Razem: ',
            avg: 'Srednia: ',
            min: 'Min: ',
            max: 'Max: '
        },
        pinning: {
          pinLeft: 'Przypnij do lewej',
          pinRight: 'Przypnij do prawej',
          unpin: 'Odepnij'
        },
        columnMenu: {
          close: 'Zamknij'
        },
        gridMenu: {
          aria: {
            buttonLabel: 'Opcje tabeli'
          },
          columns: 'Kolumny:',
          importerTitle: 'Importuj plik',
          exporterAllAsCsv: 'Eksportuj wszystkie dane do csv',
          exporterVisibleAsCsv: 'Eksportuj widoczne dane do csv',
          exporterSelectedAsCsv: 'Eksportuj zaznaczone dane do csv',
          exporterAllAsPdf: 'Eksportuj wszystkie dane do pdf',
          exporterVisibleAsPdf: 'Eksportuj widoczne dane do pdf',
          exporterSelectedAsPdf: 'Eksportuj zaznaczone dane do pdf',
          exporterAllAsExcel: 'Eksportuj wszystkie dane do excel',
          exporterVisibleAsExcel: 'Eksportuj widoczne dane do excel',
          exporterSelectedAsExcel: 'Eksportuj zaznaczone dane do excel',
          clearAllFilters: 'Wyczysc filtry'
        },
        importer: {
          noHeaders: 'Nie udalo sie wczytac nazw kolumn. Czy plik posiada naglówek?',
          noObjects: 'Nie udalo sie wczytac pozycji. Czy plik zawiera dane?',
          invalidCsv: 'Nie udalo sie przetworzyc pliku. Czy to prawidlowy plik CSV?',
          invalidJson: 'Nie udalo sie przetworzyc pliku. Czy to prawidlowy plik JSON?',
          jsonNotArray: 'Importowany plik JSON musi zawierac tablice. Importowanie przerwane.'
        },
        pagination: {
          aria: {
            pageToFirst: 'Pierwsza strona',
            pageBack: 'Poprzednia strona',
            pageSelected: 'Wybrana strona',
            pageForward: 'Nastepna strona',
            pageToLast: 'Ostatnia strona'
          },
          sizes: 'pozycji na strone',
          totalItems: 'pozycji',
          through: 'do',
          of: 'z'
        },
        grouping: {
          group: 'Grupuj',
          ungroup: 'Rozgrupuj',
          aggregate_count: 'Zbiorczo: Razem',
          aggregate_sum: 'Zbiorczo: Suma',
          aggregate_max: 'Zbiorczo: Max',
          aggregate_min: 'Zbiorczo: Min',
          aggregate_avg: 'Zbiorczo: Srednia',
          aggregate_remove: 'Zbiorczo: Usun'
        },
        validate: {
          error: 'Blad:',
          minLength: 'Wartosc powinna skladac sie z co najmniej THRESHOLD znaków.',
          maxLength: 'Wartosc powinna skladac sie z przynajmniej THRESHOLD znaków.',
          required: 'Wartosc jest wymagana.'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('pt-br', {
        headerCell: {
          aria: {
            defaultFilterLabel: 'Filtro por coluna',
            removeFilter: 'Remover filtro',
            columnMenuButtonLabel: 'Menu coluna',
            column: 'Coluna'
          },
          priority: 'Prioridade:',
          filterLabel: "Filtro por coluna: "
        },
        aggregate: {
          label: 'itens'
        },
        groupPanel: {
          description: 'Arraste e solte uma coluna aqui para agrupar por essa coluna'
        },
        search: {
          aria: {
            selected: 'Linha selecionada',
            notSelected: 'Linha não está selecionada'
          },
          placeholder: 'Procurar...',
          showingItems: 'Mostrando os Itens:',
          selectedItems: 'Items Selecionados:',
          totalItems: 'Total de Itens:',
          size: 'Tamanho da Página:',
          first: 'Primeira Página',
          next: 'Próxima Página',
          previous: 'Página Anterior',
          last: 'Última Página'
        },
        menu: {
          text: 'Selecione as colunas:'
        },
        sort: {
          ascending: 'Ordenar Ascendente',
          descending: 'Ordenar Descendente',
          none: 'Nenhuma Ordem',
          remove: 'Remover Ordenação'
        },
        column: {
          hide: 'Esconder coluna'
        },
        aggregation: {
          count: 'total de linhas: ',
          sum: 'total: ',
          avg: 'med: ',
          min: 'min: ',
          max: 'max: '
        },
        pinning: {
          pinLeft: 'Fixar Esquerda',
          pinRight: 'Fixar Direita',
          unpin: 'Desprender'
        },
        columnMenu: {
          close: 'Fechar'
        },
        gridMenu: {
          aria: {
            buttonLabel: 'Menu Grid'
          },
          columns: 'Colunas:',
          importerTitle: 'Importar arquivo',
          exporterAllAsCsv: 'Exportar todos os dados como csv',
          exporterVisibleAsCsv: 'Exportar dados visíveis como csv',
          exporterSelectedAsCsv: 'Exportar dados selecionados como csv',
          exporterAllAsPdf: 'Exportar todos os dados como pdf',
          exporterVisibleAsPdf: 'Exportar dados visíveis como pdf',
          exporterSelectedAsPdf: 'Exportar dados selecionados como pdf',
          exporterAllAsExcel: 'Exportar todos os dados como excel',
          exporterVisibleAsExcel: 'Exportar dados visíveis como excel',
          exporterSelectedAsExcel: 'Exportar dados selecionados como excel',
          clearAllFilters: 'Limpar todos os filtros'
        },
        importer: {
          noHeaders: 'Nomes de colunas não puderam ser derivados. O arquivo tem um cabeçalho?',
          noObjects: 'Objetos não puderam ser derivados. Havia dados no arquivo, além dos cabeçalhos?',
          invalidCsv: 'Arquivo não pode ser processado. É um CSV válido?',
          invalidJson: 'Arquivo não pode ser processado. É um Json válido?',
          jsonNotArray: 'Arquivo json importado tem que conter um array. Abortando.'
        },
        pagination: {
          aria: {
            pageToFirst: 'Primeira página',
            pageBack: 'Página anterior',
            pageSelected: 'Página Selecionada',
            pageForward: 'Proxima',
            pageToLast: 'Anterior'
          },
          sizes: 'itens por página',
          totalItems: 'itens',
          through: 'através dos',
          of: 'de'
        },
        grouping: {
          group: 'Agrupar',
          ungroup: 'Desagrupar',
          aggregate_count: 'Agr: Contar',
          aggregate_sum: 'Agr: Soma',
          aggregate_max: 'Agr: Max',
          aggregate_min: 'Agr: Min',
          aggregate_avg: 'Agr: Med',
          aggregate_remove: 'Agr: Remover'
        },
        validate: {
          error: 'Erro:',
          minLength: 'O valor deve ter, no minimo, THRESHOLD caracteres.',
          maxLength: 'O valor deve ter, no máximo, THRESHOLD caracteres.',
          required: 'Um valor é necessário.'
        }
      });
      return $delegate;
    }]);
}]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('pt', {
        headerCell: {
          aria: {
            defaultFilterLabel: 'Filtro por coluna',
            removeFilter: 'Remover filtro',
            columnMenuButtonLabel: 'Menu coluna',
            column: 'Coluna'
          },
          priority: 'Prioridade:',
          filterLabel: "Filtro por coluna: "
        },
        aggregate: {
          label: 'itens'
        },
        groupPanel: {
          description: 'Arraste e solte uma coluna aqui para agrupar por essa coluna'
        },
        search: {
          aria: {
            selected: 'Linha selecionada',
            notSelected: 'Linha não está selecionada'
          },
          placeholder: 'Procurar...',
          showingItems: 'Mostrando os Itens:',
          selectedItems: 'Itens Selecionados:',
          totalItems: 'Total de Itens:',
          size: 'Tamanho da Página:',
          first: 'Primeira Página',
          next: 'Próxima Página',
          previous: 'Página Anterior',
          last: 'Última Página'
        },
        menu: {
          text: 'Selecione as colunas:'
        },
        sort: {
          ascending: 'Ordenar Ascendente',
          descending: 'Ordenar Descendente',
          none: 'Nenhuma Ordem',
          remove: 'Remover Ordenação'
        },
        column: {
          hide: 'Esconder coluna'
        },
        aggregation: {
          count: 'total de linhas: ',
          sum: 'total: ',
          avg: 'med: ',
          min: 'min: ',
          max: 'max: '
        },
        pinning: {
          pinLeft: 'Fixar Esquerda',
          pinRight: 'Fixar Direita',
          unpin: 'Desprender'
        },
        columnMenu: {
          close: 'Fechar'
        },
        gridMenu: {
          aria: {
            buttonLabel: 'Menu Grid'
          },
          columns: 'Colunas:',
          importerTitle: 'Importar ficheiro',
          exporterAllAsCsv: 'Exportar todos os dados como csv',
          exporterVisibleAsCsv: 'Exportar dados visíveis como csv',
          exporterSelectedAsCsv: 'Exportar dados selecionados como csv',
          exporterAllAsPdf: 'Exportar todos os dados como pdf',
          exporterVisibleAsPdf: 'Exportar dados visíveis como pdf',
          exporterSelectedAsPdf: 'Exportar dados selecionados como pdf',
          exporterAllAsExcel: 'Exportar todos os dados como excel',
          exporterVisibleAsExcel: 'Exportar dados visíveis como excel',
          exporterSelectedAsExcel: 'Exportar dados selecionados como excel',
          clearAllFilters: 'Limpar todos os filtros'
        },
        importer: {
          noHeaders: 'Nomes de colunas não puderam ser derivados. O ficheiro tem um cabeçalho?',
          noObjects: 'Objetos não puderam ser derivados. Havia dados no ficheiro, além dos cabeçalhos?',
          invalidCsv: 'Ficheiro não pode ser processado. É um CSV válido?',
          invalidJson: 'Ficheiro não pode ser processado. É um Json válido?',
          jsonNotArray: 'Ficheiro json importado tem que conter um array. Interrompendo.'
        },
        pagination: {
          aria: {
            pageToFirst: 'Primeira página',
            pageBack: 'Página anterior',
            pageSelected: 'Página Selecionada',
            pageForward: 'Próxima',
            pageToLast: 'Anterior'
          },
          sizes: 'itens por página',
          totalItems: 'itens',
          through: 'a',
          of: 'de'
        },
        grouping: {
          group: 'Agrupar',
          ungroup: 'Desagrupar',
          aggregate_count: 'Agr: Contar',
          aggregate_sum: 'Agr: Soma',
          aggregate_max: 'Agr: Max',
          aggregate_min: 'Agr: Min',
          aggregate_avg: 'Agr: Med',
          aggregate_remove: 'Agr: Remover'
        },
        validate: {
          error: 'Erro:',
          minLength: 'O valor deve ter, no minimo, THRESHOLD caracteres.',
          maxLength: 'O valor deve ter, no máximo, THRESHOLD caracteres.',
          required: 'Um valor é necessário.'
        }
      });
      return $delegate;
    }]);
}]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('ro', {
        headerCell: {
          aria: {
            defaultFilterLabel: 'Filtru pentru coloana',
            removeFilter: 'Sterge filtru',
            columnMenuButtonLabel: 'Column Menu'
          },
          priority: 'Prioritate:',
          filterLabel: "Filtru pentru coloana:"
        },
        aggregate: {
          label: 'Elemente'
        },
        groupPanel: {
          description: 'Trage un cap de coloana aici pentru a grupa elementele dupa coloana respectiva'
        },
        search: {
          placeholder: 'Cauta...',
          showingItems: 'Arata elementele:',
          selectedItems: 'Elementele selectate:',
          totalItems: 'Total elemente:',
          size: 'Marime pagina:',
          first: 'Prima pagina',
          next: 'Pagina urmatoare',
          previous: 'Pagina anterioara',
          last: 'Ultima pagina'
        },
        menu: {
          text: 'Alege coloane:'
        },
        sort: {
          ascending: 'Ordoneaza crescator',
          descending: 'Ordoneaza descrescator',
          none: 'Fara ordonare',
          remove: 'Sterge ordonarea'
        },
        column: {
          hide: 'Ascunde coloana'
        },
        aggregation: {
          count: 'total linii: ',
          sum: 'total: ',
          avg: 'medie: ',
          min: 'min: ',
          max: 'max: '
        },
        pinning: {
          pinLeft: 'Pin la stanga',
          pinRight: 'Pin la dreapta',
          unpin: 'Sterge pinul'
        },
        columnMenu: {
          close: 'Inchide'
        },
        gridMenu: {
          aria: {
            buttonLabel: 'Grid Menu'
          },
          columns: 'Coloane:',
          importerTitle: 'Incarca fisier',
          exporterAllAsCsv: 'Exporta toate datele ca csv',
          exporterVisibleAsCsv: 'Exporta datele vizibile ca csv',
          exporterSelectedAsCsv: 'Exporta datele selectate ca csv',
          exporterAllAsPdf: 'Exporta toate datele ca pdf',
          exporterVisibleAsPdf: 'Exporta datele vizibile ca pdf',
          exporterSelectedAsPdf: 'Exporta datele selectate ca csv pdf',
          clearAllFilters: 'Sterge toate filtrele'
        },
        importer: {
          noHeaders: 'Numele coloanelor nu a putut fi incarcat, acest fisier are un header?',
          noObjects: 'Datele nu au putut fi incarcate, exista date in fisier in afara numelor de coloane?',
          invalidCsv: 'Fisierul nu a putut fi procesat, ati incarcat un CSV valid ?',
          invalidJson: 'Fisierul nu a putut fi procesat, ati incarcat un Json valid?',
          jsonNotArray: 'Json-ul incarcat trebuie sa contina un array, inchidere.'
        },
        pagination: {
          aria: {
            pageToFirst: 'Prima pagina',
            pageBack: 'O pagina inapoi',
            pageSelected: 'Pagina selectata',
            pageForward: 'O pagina inainte',
            pageToLast: 'Ultima pagina'
          },
          sizes: 'Elemente per pagina',
          totalItems: 'elemente',
          through: 'prin',
          of: 'of'
        },
        grouping: {
          group: 'Grupeaza',
          ungroup: 'Opreste gruparea',
          aggregate_count: 'Agg: Count',
          aggregate_sum: 'Agg: Sum',
          aggregate_max: 'Agg: Max',
          aggregate_min: 'Agg: Min',
          aggregate_avg: 'Agg: Avg',
          aggregate_remove: 'Agg: Remove'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function() {
	angular.module('ui.grid').config(['$provide', function($provide) {
		$provide.decorator('i18nService', ['$delegate', function($delegate) {
			$delegate.add('rs-lat', {
				headerCell: {
					aria: {
						defaultFilterLabel: 'Filter za kolonu',
						removeFilter: 'Ukloni Filter',
						columnMenuButtonLabel: 'Meni Kolone',
						column: 'Kolona'
					},
					priority: 'Prioritet:',
					filterLabel: "Filter za kolonu: "
				},
				aggregate: {
					label: 'stavke'
				},
				groupPanel: {
					description: 'Ovde prevuci zaglavlje kolone i spusti do grupe pored te kolone.'
				},
				search: {
					aria: {
						selected: 'Red odabran',
						notSelected: 'Red nije odabran'
					},
					placeholder: 'Pretraga...',
					showingItems: 'Prikazane Stavke:',
					selectedItems: 'Odabrane Stavke:',
					totalItems: 'Ukupno Stavki:',
					size: 'Velicina Stranice:',
					first: 'Prva Stranica',
					next: 'Sledeca Stranica',
					previous: 'Prethodna Stranica',
					last: 'Poslednja Stranica'
				},
				menu: {
					text: 'Odaberite kolonu:'
				},
				sort: {
					ascending: 'Sortiraj po rastucem redosledu',
					descending: 'Sortiraj po opadajucem redosledu',
					none: 'Bez Sortiranja',
					remove: 'Ukloni Sortiranje'
				},
				column: {
					hide: 'Sakrij Kolonu'
				},
				aggregation: {
					count: 'ukupno redova: ',
					sum: 'ukupno: ',
					avg: 'prosecno: ',
					min: 'minimum: ',
					max: 'maksimum: '
				},
				pinning: {
					pinLeft: 'Zakaci Levo',
					pinRight: 'Zakaci Desno',
					unpin: 'Otkaci'
				},
				columnMenu: {
					close: 'Zatvori'
				},
				gridMenu: {
					aria: {
						buttonLabel: 'Rešetkasti Meni'
					},
					columns: 'Kolone:',
					importerTitle: 'Importuj fajl',
					exporterAllAsCsv: 'Eksportuj sve podatke kao csv',
					exporterVisibleAsCsv: 'Eksportuj vidljive podatke kao csv',
					exporterSelectedAsCsv: 'Eksportuj obeležene podatke kao csv',
					exporterAllAsPdf: 'Eksportuj sve podatke kao pdf',
					exporterVisibleAsPdf: 'Eksportuj vidljive podake kao pdf',
					exporterSelectedAsPdf: 'Eksportuj odabrane podatke kao pdf',
					exporterAllAsExcel: 'Eksportuj sve podatke kao excel',
					exporterVisibleAsExcel: 'Eksportuj vidljive podatke kao excel',
					exporterSelectedAsExcel: 'Eksportuj odabrane podatke kao excel',
					clearAllFilters: 'Obriši sve filtere'
				},
				importer: {
					noHeaders: 'Kolone se nisu mogle podeliti, da li fajl poseduje heder?',
					noObjects: 'Objecti nisu mogli biti podeljeni, da li je bilo i drugih podataka sem hedera?',
					invalidCsv: 'Fajl nije bilo moguce procesirati, da li je ispravni CSV?',
					invalidJson: 'Fajl nije bilo moguce procesirati, da li je ispravni JSON',
					jsonNotArray: 'Importovani json fajl mora da sadrži niz, prekidam operaciju.'
				},
				pagination: {
					aria: {
						pageToFirst: 'Prva stranica',
						pageBack: 'Stranica pre',
						pageSelected: 'Odabrana stranica',
						pageForward: 'Sledeca stranica',
						pageToLast: 'Poslednja stranica'
					},
					sizes: 'stavki po stranici',
					totalItems: 'stavke',
					through: 'kroz',
					of: 'od'
				},
				grouping: {
					group: 'Grupiši',
					ungroup: 'Odrupiši',
					aggregate_count: 'Agg: Broj',
					aggregate_sum: 'Agg: Suma',
					aggregate_max: 'Agg: Maksimum',
					aggregate_min: 'Agg: Minimum',
					aggregate_avg: 'Agg: Prosecna',
					aggregate_remove: 'Agg: Ukloni'
				},
				validate: {
					error: 'Greška:',
					minLength: 'Vrednost bi trebala da bude duga bar THRESHOLD karaktera.',
					maxLength: 'Vrednost bi trebalo da bude najviše duga THRESHOLD karaktera.',
					required: 'Portreba je vrednost.'
				}
			});
			return $delegate;
		}]);
	}]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('ru', {
        headerCell: {
          aria: {
            defaultFilterLabel: '?????? ???????',
            removeFilter: '??????? ??????',
            columnMenuButtonLabel: '???? ???????'
          },
          priority: '?????????:',
          filterLabel: "?????? ???????: "
        },
        aggregate: {
          label: '????????'
        },
        groupPanel: {
          description: '??? ??????????? ?? ??????? ?????????? ???? ??? ????????.'
        },
        search: {
          placeholder: '?????...',
          showingItems: '???????? ????????:',
          selectedItems: '????????? ????????:',
          totalItems: '????? ?????????:',
          size: '?????? ????????:',
          first: '?????? ????????',
          next: '????????? ????????',
          previous: '?????????? ????????',
          last: '????????? ????????'
        },
        menu: {
          text: '??????? ???????:'
        },
        sort: {
          ascending: '?? ???????????',
          descending: '?? ????????',
          none: '??? ??????????',
          remove: '?????? ??????????'
        },
        column: {
          hide: '???????? ???????'
        },
        aggregation: {
          count: '????? ?????: ',
          sum: '?????: ',
          avg: '???????: ',
          min: '???: ',
          max: '????: '
        },
				pinning: {
					pinLeft: '????????? ?????',
					pinRight: '????????? ??????',
					unpin: '?????????'
				},
        columnMenu: {
          close: '???????'
        },
        gridMenu: {
          aria: {
            buttonLabel: '????'
          },
          columns: '???????:',
          importerTitle: '????????????? ????',
          exporterAllAsCsv: '?????????????? ??? ? CSV',
          exporterVisibleAsCsv: '?????????????? ??????? ?????? ? CSV',
          exporterSelectedAsCsv: '?????????????? ????????? ?????? ? CSV',
          exporterAllAsPdf: '?????????????? ??? ? PDF',
          exporterVisibleAsPdf: '?????????????? ??????? ?????? ? PDF',
          exporterSelectedAsPdf: '?????????????? ????????? ?????? ? PDF',
          exporterAllAsExcel: '?????????????? ??? ? Excel',
          exporterVisibleAsExcel: '?????????????? ??????? ?????? ? Excel',
          exporterSelectedAsExcel: '?????????????? ????????? ?????? ? Excel',
          clearAllFilters: '???????? ??? ???????'
        },
        importer: {
          noHeaders: '?? ??????? ???????? ???????? ????????, ???? ?? ? ????? ??????????',
          noObjects: '?? ??????? ???????? ??????, ???? ?? ? ????? ?????? ????? ??????????',
          invalidCsv: '?? ??????? ?????????? ????, ??? ?????????? CSV-?????',
          invalidJson: '?? ??????? ?????????? ????, ??? ?????????? JSON?',
          jsonNotArray: '????????????? JSON-???? ?????? ????????? ??????, ???????? ????????.'
        },
        pagination: {
          aria: {
            pageToFirst: '?????? ????????',
            pageBack: '?????????? ????????',
            pageSelected: '????????? ????????',
            pageForward: '????????? ????????',
            pageToLast: '????????? ????????'
          },
          sizes: '????? ?? ????????',
          totalItems: '?????',
          through: '??',
          of: '??'
        },
        grouping: {
          group: '????????????',
          ungroup: '???????????????',
          aggregate_count: '????????????: Count',
          aggregate_sum: '??? ??????: ?????',
          aggregate_max: '??? ??????: ????????',
          aggregate_min: '??? ??????: ???????',
          aggregate_avg: '??? ??????: ???????',
          aggregate_remove: '??? ??????: ?????'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function() {
	angular.module('ui.grid').config(['$provide', function($provide) {
		$provide.decorator('i18nService', ['$delegate', function($delegate) {
			$delegate.add('sk', {
				headerCell: {
					aria: {
						defaultFilterLabel: 'Filter pre stlpec',
						removeFilter: 'Odstránit filter',
						columnMenuButtonLabel: 'Menu pre stlpec',
						column: 'Stlpec'
					},
					priority: 'Priorita:',
					filterLabel: "Filter pre stlpec: "
				},
				aggregate: {
					label: 'položky'
				},
				groupPanel: {
					description: 'Pretiahni sem názov stlpca pre zoskupenie podla toho stlpca.'
				},
				search: {
					aria: {
						selected: 'Oznacený riadok',
						notSelected: 'Neoznacený riadok'
					},
					placeholder: 'Hladaj...',
					showingItems: 'Zobrazujem položky:',
					selectedItems: 'Vybraté položky:',
					totalItems: 'Pocet položiek:',
					size: 'Pocet:',
					first: 'Prvá strana',
					next: 'Dalšia strana',
					previous: 'Predchádzajúca strana',
					last: 'Posledná strana'
				},
				menu: {
					text: 'Vyberte stlpce:'
				},
				sort: {
					ascending: 'Zotriedit vzostupne',
					descending: 'Zotriedit zostupne',
					none: 'Nezotriedit',
					remove: 'Vymazat triedenie'
				},
				column: {
					hide: 'Skryt stlpec'
				},
				aggregation: {
					count: 'pocet riadkov: ',
					sum: 'spolu: ',
					avg: 'avg: ',
					min: 'min: ',
					max: 'max: '
				},
				pinning: {
					pinLeft: 'Pripnút vlavo',
					pinRight: 'Pripnút vpravo',
					unpin: 'Odopnút'
				},
				columnMenu: {
					close: 'Zavriet'
				},
				gridMenu: {
					aria: {
						buttonLabel: 'Grid Menu'
					},
					columns: 'Stlpce:',
					importerTitle: 'Importovat súbor',
					exporterAllAsCsv: 'Exportovat všetky údaje ako CSV',
					exporterVisibleAsCsv: 'Exportovt viditelné údaje ako CSV',
					exporterSelectedAsCsv: 'Exportovat oznacené údaje ako CSV',
					exporterAllAsPdf: 'Exportovat všetky údaje ako pdf',
					exporterVisibleAsPdf: 'Exportovat viditelné údaje ako pdf',
					exporterSelectedAsPdf: 'Exportovat oznacené údaje ako pdf',
					exporterAllAsExcel: 'Exportovat všetky údaje ako excel',
					exporterVisibleAsExcel: 'Exportovat viditelné údaje ako excel',
					exporterSelectedAsExcel: 'Exportovat oznacené údaje ako excel',
					clearAllFilters: 'Zrušit všetky filtre'
				},
				importer: {
					noHeaders: 'Názvy stlpcov sa nedali odvodit, má súbor hlavicku?',
					noObjects: 'Objekty nebolo možné odvodit, existovali iné údaje v súbore ako hlavicky?',
					invalidCsv: 'Súbor sa nepodarilo spracovat, je to platný súbor CSV?',
					invalidJson: 'Súbor nebolo možné spracovat, je to platný súbor typu Json?',
					jsonNotArray: 'Importovaný súbor json musí obsahovat pole, ukoncujem.'
				},
				pagination: {
					aria: {
						pageToFirst: 'Strana na zaciatok',
						pageBack: 'Strana dozadu',
						pageSelected: 'Oznacená strana',
						pageForward: 'Strana dopredu',
						pageToLast: 'Strana na koniec'
					},
					sizes: 'položky na stranu',
					totalItems: 'položky spolu',
					through: 'do konca',
					of: 'z'
				},
				grouping: {
					group: 'Zoskupit',
					ungroup: 'Zrušit zoskupenie',
					aggregate_count: 'Agg: Pocet',

					aggregate_sum: 'Agg: Suma',
					aggregate_max: 'Agg: Max',
					aggregate_min: 'Agg: Min',
					aggregate_avg: 'Agg: Avg',
					aggregate_remove: 'Agg: Zrušit'
				},
				validate: {
					error: 'Chyba:',
					minLength: 'Hodnota by mala mat aspon THRESHOLD znakov dlhá.',
					maxLength: 'Hodnota by mala byt maximálne THRESHOLD znakov dlhá.',
					required: 'Vyžaduje sa hodnota.'
				}
			});
			return $delegate;
		}]);
	}]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('sv', {
        headerCell: {
          aria: {
            defaultFilterLabel: 'Kolumnfilter',
            removeFilter: 'Ta bort filter',
            columnMenuButtonLabel: 'Kolumnmeny',
            column: 'Kolumn'
          },
          priority: 'Prioritet:',
          filterLabel: "Filter för kolumn: "
        },
        aggregate: {
          label: 'Poster'
        },
        groupPanel: {
          description: 'Dra en kolumnrubrik hit och släpp den för att gruppera efter den kolumnen.'
        },
        search: {
          aria: {
            selected: 'Rad är vald',
            notSelected: 'Rad är inte vald'
          },
          placeholder: 'Sök...',
          showingItems: 'Visar:',
          selectedItems: 'Valda:',
          totalItems: 'Antal:',
          size: 'Sidstorlek:',
          first: 'Första sidan',
          next: 'Nästa sida',
          previous: 'Föregående sida',
          last: 'Sista sidan'
        },
        menu: {
          text: 'Välj kolumner:'
        },
        sort: {
          ascending: 'Sortera stigande',
          descending: 'Sortera fallande',
          none: 'Ingen sortering',
          remove: 'Inaktivera sortering'
        },
        column: {
          hide: 'Göm kolumn'
        },
        aggregation: {
          count: 'Antal rader: ',
          sum: 'Summa: ',
          avg: 'Genomsnitt: ',
          min: 'Min: ',
          max: 'Max: '
        },
        pinning: {
          pinLeft: 'Fäst vänster',
          pinRight: 'Fäst höger',
          unpin: 'Lösgör'
        },
        columnMenu: {
          close: 'Stäng'
        },
        gridMenu: {
          aria: {
              buttonLabel: 'Meny'
          },
          columns: 'Kolumner:',
          importerTitle: 'Importera fil',
          exporterAllAsCsv: 'Exportera all data som CSV',
          exporterVisibleAsCsv: 'Exportera synlig data som CSV',
          exporterSelectedAsCsv: 'Exportera markerad data som CSV',
          exporterAllAsPdf: 'Exportera all data som PDF',
          exporterVisibleAsPdf: 'Exportera synlig data som PDF',
          exporterSelectedAsPdf: 'Exportera markerad data som PDF',
          exporterAllAsExcel: 'Exportera all data till Excel',
          exporterVisibleAsExcel: 'Exportera synlig data till Excel',
          exporterSelectedAsExcel: 'Exportera markerad data till Excel',
          clearAllFilters: 'Nollställ alla filter'
        },
        importer: {
          noHeaders: 'Kolumnnamn kunde inte härledas. Har filen ett sidhuvud?',
          noObjects: 'Objekt kunde inte härledas. Har filen data undantaget sidhuvud?',
          invalidCsv: 'Filen kunde inte behandlas, är den en giltig CSV?',
          invalidJson: 'Filen kunde inte behandlas, är den en giltig JSON?',
          jsonNotArray: 'Importerad JSON-fil måste innehålla ett fält. Import avbruten.'
        },
        pagination: {
          aria: {
            pageToFirst: 'Gå till första sidan',
            pageBack: 'Gå en sida bakåt',
            pageSelected: 'Vald sida',
            pageForward: 'Gå en sida framåt',
            pageToLast: 'Gå till sista sidan'
          },
          sizes: 'Poster per sida',
          totalItems: 'Poster',
          through: 'genom',
          of: 'av'
        },
        grouping: {
          group: 'Gruppera',
          ungroup: 'Dela upp',
          aggregate_count: 'Agg: Antal',
          aggregate_sum: 'Agg: Summa',
          aggregate_max: 'Agg: Max',
          aggregate_min: 'Agg: Min',
          aggregate_avg: 'Agg: Genomsnitt',
          aggregate_remove: 'Agg: Ta bort'
        },
        validate: {
          error: 'Error:',
          minLength: 'Värdet borde vara minst THRESHOLD tecken långt.',
          maxLength: 'Värdet borde vara max THRESHOLD tecken långt.',
          required: 'Ett värde krävs.'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('ta', {
        aggregate: {
          label: '???????????'
        },
        groupPanel: {
          description: '??? ??????? ??????? ?????? ???????????? ??????? ?????  ??????? ?????? '
        },
        search: {
          placeholder: '????? ...',
          showingItems: '??????????? ???????????:',
          selectedItems: '???????????????????  ???????????:',
          totalItems: '????? ???????????:',
          size: '???? ????: ',
          first: '????? ??????',
          next: '?????? ??????',
          previous: '??????? ?????? ',
          last: '????? ??????'
        },
        menu: {
          text: '???????? ??????????:'
        },
        sort: {
          ascending: '?????????? ?????',
          descending: '?????????? ?????',
          remove: '??????? ??????'
        },
        column: {
          hide: '??????? ??????? ?? '
        },
        aggregation: {
          count: '????? ??????:',
          sum: '???????: ',
          avg: '??????: ',
          min: '???????????: ',
          max: '????????: '
        },
        pinning: {
         pinLeft: '?????????? ????? ',
          pinRight: '?????????? ?????',
          unpin: '????'
        },
        gridMenu: {
          columns: '????????:',
          importerTitle: '?????? : ????????',
          exporterAllAsCsv: '????? ??????????? ??????????: csv',
          exporterVisibleAsCsv: '????????? ??????? ??????????: csv',
          exporterSelectedAsCsv: '????????????? ??????? ??????????: csv',
          exporterAllAsPdf: '????? ??????????? ??????????: pdf',
          exporterVisibleAsPdf: '????????? ??????? ??????????: pdf',
          exporterSelectedAsPdf: '????????????? ??????? ??????????: pdf',
          clearAllFilters: 'Clear all filters'
        },
        importer: {
          noHeaders: '????????? ?????????? ??? ?????????, ?????????? ??????? ???????',
          noObjects: '????????? ???????? ???????????, ???????? ?????????? ???? ???? ??????? ??????? ',
          invalidCsv:	'????? ??????? ?????? ?????????, ?????? ???????? - csv',
          invalidJson: '????? ??????? ?????? ?????????, ?????? ???????? - json',
          jsonNotArray: '?????? ???????? ???????? ??????, ??????? ????? ???? : json'
        },
        pagination: {
          sizes		: '??????????? / ??????',
          totalItems	: '??????????? '
        },
        grouping: {
          group	: '????',
          ungroup : '????',
          aggregate_count	: '??????????? : ?????',
          aggregate_sum : '??????????? : ???????',
          aggregate_max	: '??????????? : ??????????',
          aggregate_min	: '??????????? : ?????????????',
          aggregate_avg	: '??????????? : ??????',
          aggregate_remove : '??????????? : ??????'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function() {
	angular.module('ui.grid').config(['$provide', function($provide) {
		$provide.decorator('i18nService', ['$delegate', function($delegate) {
			$delegate.add('tr', {
				headerCell: {
					aria: {
						defaultFilterLabel: 'Sütun için filtre',
						removeFilter: 'Filtreyi Kaldir',
						columnMenuButtonLabel: 'Sütun Menüsü'
					},
					priority: 'Öncelik:',
					filterLabel: "Sütun için filtre: "
				},
				aggregate: {
					label: 'kayitlar'
				},
				groupPanel: {
					description: 'Sütuna göre gruplamak için sütun basligini buraya sürükleyin ve birakin.'
				},
				search: {
					placeholder: 'Arama...',
					showingItems: 'Gösterilen Kayit:',
					selectedItems: 'Seçili Kayit:',
					totalItems: 'Toplam Kayit:',
					size: 'Sayfa Boyutu:',
					first: 'Ilk Sayfa',
					next: 'Sonraki Sayfa',
					previous: 'Önceki Sayfa',
					last: 'Son Sayfa'
				},
				menu: {
					text: 'Sütunlari Seç:'
				},
				sort: {
					ascending: 'Artan Sirada Sirala',
					descending: 'Azalan Sirada Sirala',
					none: 'Siralama Yapma',
					remove: 'Siralamayi Kaldir'
				},
				column: {
					hide: 'Sütunu Gizle'
				},
				aggregation: {
					count: 'toplam satir: ',
					sum: 'toplam: ',
					avg: 'ort: ',
					min: 'min: ',
					max: 'maks: '
				},
				pinning: {
					pinLeft: 'Sola Sabitle',
					pinRight: 'Saga Sabitle',
					unpin: 'Sabitlemeyi Kaldir'
				},
				columnMenu: {
					close: 'Kapat'
				},
				gridMenu: {
					aria: {
						buttonLabel: 'Tablo Menü'
					},
					columns: 'Sütunlar:',
					importerTitle: 'Dosya içeri aktar',
					exporterAllAsCsv: 'Bütün veriyi CSV olarak disari aktar',
					exporterVisibleAsCsv: 'Görünen veriyi CSV olarak disari aktar',
					exporterSelectedAsCsv: 'Seçili veriyi CSV olarak disari aktar',
					exporterAllAsPdf: 'Bütün veriyi PDF olarak disari aktar',
					exporterVisibleAsPdf: 'Görünen veriyi PDF olarak disari aktar',
					exporterSelectedAsPdf: 'Seçili veriyi PDF olarak disari aktar',
					clearAllFilters: 'Bütün filtreleri kaldir'
				},
				importer: {
					noHeaders: 'Sütun isimleri üretilemiyor, dosyanin bir basligi var mi?',
					noObjects: 'Nesneler üretilemiyor, dosyada basliktan baska bir veri var mi?',
					invalidCsv: 'Dosya islenemedi, geçerli bir CSV dosyasi mi?',
					invalidJson: 'Dosya islenemedi, geçerli bir Json dosyasi mi?',
					jsonNotArray: 'Alinan Json dosyasinda bir dizi bulunmalidir, islem iptal ediliyor.'
				},
				pagination: {
					aria: {
						pageToFirst: 'Ilk sayfaya',
						pageBack: 'Geri git',
						pageSelected: 'Seçili sayfa',
						pageForward: 'Ileri git',
						pageToLast: 'Sona git'
					},
					sizes: 'Sayfadaki nesne sayisi',
					totalItems: 'kayitlar',
					through: '', // note(fsw) : turkish dont have this preposition
					of: '' // note(fsw) : turkish dont have this preposition
				},
				grouping: {
					group: 'Grupla',
					ungroup: 'Gruplama',
					aggregate_count: 'Yekun: Sayi',
					aggregate_sum: 'Yekun: Toplam',
					aggregate_max: 'Yekun: Maks',
					aggregate_min: 'Yekun: Min',
					aggregate_avg: 'Yekun: Ort',
					aggregate_remove: 'Yekun: Sil'
				}
			});
			return $delegate;
		}]);
	}]);
})();

(function () {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('ua', {
        headerCell: {
          aria: {
            defaultFilterLabel: '?????? ?????????',
            removeFilter: '???????? ??????',
            columnMenuButtonLabel: '???? ????????'
          },
          priority: '?????????:',
          filterLabel: "?????? ?????????: "
        },
        aggregate: {
          label: '????????'
        },
        groupPanel: {
          description: '??? ?????????? ?? ?????????? ??????????? ???? ???? ?????.'
        },
        search: {
          placeholder: '?????...',
          showingItems: '???????? ????????:',
          selectedItems: '?????? ????????:',
          totalItems: '?????? ?????????:',
          size: '?????? ????????:',
          first: '????? ????????',
          next: '???????? ????????',
          previous: '????????? ????????',
          last: '??????? ????????'
        },
        menu: {
          text: '?????? ????????:'
        },
        sort: {
          ascending: '?? ??????????',
          descending: '?? ?????????',
          none: '??? ??????????',
          remove: '???????? ??????????'
        },
        column: {
          hide: '????????? ????????'
        },
        aggregation: {
          count: '?????? ??????: ',
          sum: '?????: ',
          avg: '???????: ',
          min: '???: ',
          max: '????: '
        },
				pinning: {
					pinLeft: '????????? ???????',
					pinRight: '????????? ????????',
					unpin: '??????????'
				},
        columnMenu: {
          close: '???????'
        },
        gridMenu: {
          aria: {
            buttonLabel: '????'
          },
          columns: '?????????:',
          importerTitle: '??????????? ????',
          exporterAllAsCsv: '???????????? ??? ? CSV',
          exporterVisibleAsCsv: '???????????? ?????? ???? ? CSV',
          exporterSelectedAsCsv: '???????????? ?????? ???? ? CSV',
          exporterAllAsPdf: '???????????? ??? ? PDF',
          exporterVisibleAsPdf: '???????????? ?????? ???? ? PDF',
          exporterSelectedAsPdf: '???????????? ?????? ???? ? PDF',
          clearAllFilters: '???????? ??? ???????'
        },
        importer: {
          noHeaders: '?? ??????? ???????? ????? ??????????, ?? ? ? ????? ??????????',
          noObjects: '?? ??????? ???????? ????, ?? ? ? ????? ????? ????? ??????????',
          invalidCsv: '?? ??????? ???????? ????, ?? ?? ????????? CSV-?????',
          invalidJson: '?? ??????? ???????? ????, ?? ?? ????????? JSON?',
          jsonNotArray: 'JSON-???? ?? ???????????? ??????? ??????? ?????, ???????? ?????????.'
        },
        pagination: {
          aria: {
            pageToFirst: '????? ????????',
            pageBack: '????????? ????????',
            pageSelected: '?????? ????????',
            pageForward: '???????? ????????',
            pageToLast: '??????? ????????'
          },
          sizes: '?????? ?? ????????',
          totalItems: '??????',
          through: '??',
          of: '?'
        },
        grouping: {
          group: '?????????',
          ungroup: '????????????',
          aggregate_count: '?????????: ?????????',
          aggregate_sum: '??? ?????: ????',
          aggregate_max: '??? ?????: ????????',
          aggregate_min: '??? ?????: ???????',
          aggregate_avg: '??? ?????: ??????',
          aggregate_remove: '??? ?????: ?????'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function() {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('zh-cn', {
        headerCell: {
          aria: {
            defaultFilterLabel: '????',
            removeFilter: '?????',
            columnMenuButtonLabel: '???'
          },
          priority: '???:',
          filterLabel: "????: "
        },
        aggregate: {
          label: '?'
        },
        groupPanel: {
          description: '???????????'
        },
        search: {
          placeholder: '??',
          showingItems: '?????:',
          selectedItems: '?????:',
          totalItems: '???:',
          size: '??????:',
          first: '??',
          next: '???',
          previous: '???',
          last: '??'
        },
        menu: {
          text: '???:'
        },
        sort: {
          ascending: '??',
          descending: '??',
          none: '??',
          remove: '????'
        },
        column: {
          hide: '???'
        },
        aggregation: {
          count: '??:',
          sum: '??:',
          avg: '??:',
          min: '???:',
          max: '???:'
        },
        pinning: {
          pinLeft: '????',
          pinRight: '????',
          unpin: '????'
        },
        columnMenu: {
          close: '??'
        },
        gridMenu: {
          aria: {
            buttonLabel: '????'
          },
          columns: '?:',
          importerTitle: '????',
          exporterAllAsCsv: '???????CSV',
          exporterVisibleAsCsv: '???????CSV',
          exporterSelectedAsCsv: '???????CSV',
          exporterAllAsPdf: '???????PDF',
          exporterVisibleAsPdf: '???????PDF',
          exporterSelectedAsPdf: '???????PDF',
          clearAllFilters: '???????'
        },
        importer: {
          noHeaders: '??????,?????????',
          noObjects: '??????,?????????',
          invalidCsv: '??????,??????CSV???',
          invalidJson: '??????,??????JSON???',
          jsonNotArray: '???????JSON??!'
        },
        pagination: {
          aria: {
            pageToFirst: '???',
            pageBack: '???',
            pageSelected: '???',
            pageForward: '???',
            pageToLast: '????'
          },
          sizes: '???',
          totalItems: '?',
          through: '?',
          of: '?'
        },
        grouping: {
          group: '??',
          ungroup: '????',
          aggregate_count: '??: ??',
          aggregate_sum: '??: ??',
          aggregate_max: '??: ??',
          aggregate_min: '??: ??',
          aggregate_avg: '??: ??',
          aggregate_remove: '??: ??'
        }
      });
      return $delegate;
    }]);
  }]);
})();

(function() {
  angular.module('ui.grid').config(['$provide', function($provide) {
    $provide.decorator('i18nService', ['$delegate', function($delegate) {
      $delegate.add('zh-tw', {
        aggregate: {
          label: '?'
        },
        groupPanel: {
          description: '???????????'
        },
        search: {
          placeholder: '??',
          showingItems: '?????:',
          selectedItems: '?????:',
          totalItems: '???:',
          size: '??????:',
          first: '??',
          next: '???',
          previous: '???',
          last: '??'
        },
        menu: {
          text: '???:'
        },
        sort: {
          ascending: '??',
          descending: '??',
          remove: '????'
        },
        column: {
          hide: '???'
        },
        aggregation: {
          count: '??:',
          sum: '??:',
          avg: '??:',
          min: '???:',
          max: '???:'
        },
        pinning: {
          pinLeft: '????',
          pinRight: '????',
          unpin: '????'
        },
        gridMenu: {
          columns: '?:',
          importerTitle: '????',
          exporterAllAsCsv: '???????CSV',
          exporterVisibleAsCsv: '???????CSV',
          exporterSelectedAsCsv: '???????CSV',
          exporterAllAsPdf: '???????PDF',
          exporterVisibleAsPdf: '???????PDF',
          exporterSelectedAsPdf: '???????PDF',
          clearAllFilters: '???????'
        },
        importer: {
          noHeaders: '??????,?????????',
          noObjects: '??????,?????????',
          invalidCsv: '??????,??????CSV???',
          invalidJson: '??????,??????JSON???',
          jsonNotArray: '???????JSON??!'
        },
        pagination: {
          sizes: '???',
          totalItems: '?'
        }
      });
      return $delegate;
    }]);
  }]);
})();