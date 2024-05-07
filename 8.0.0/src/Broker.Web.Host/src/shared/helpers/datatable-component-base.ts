//import { Input, Component, Injector, ElementRef, ViewChild, OnInit, Renderer2, ComponentFactoryResolver, ViewContainerRef, ComponentRef, ReflectiveInjector } from '@angular/core';
import { Injector, ElementRef, ViewChild, OnInit, Component, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import {
    LocalizationService,
    PermissionCheckerService,
    FeatureCheckerService,
    NotifyService,
    SettingService,
    MessageService,
    AbpMultiTenancyService
} from 'abp-ng2-module';
import { AppSessionService } from '@shared/session/app-session.service';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { DtCustomSettings } from './dtCustomSettings';
import { AppComponentBase } from '@shared/app-component-base';
import * as moment from 'moment';

import * as momenthijri from 'moment-hijri';


@Component({
    template: ''
})
export abstract class DataTableComponentBase implements OnInit {
    renderer: Renderer2;
    baseUrl: string;
    private initialParams: any[] = [];
    private ajaxParams: any[] = [];
    private router;
    stockValue: number = 0;
    totalRecords: number = 0;
    filteredCount: number = 0;
    @ViewChild(DataTableDirective)
    dtElement: DataTableDirective;
    dtOptions: DtCustomSettings = {};
    dtTrigger: Subject<any> = new Subject();
    @ViewChild('filterationArea') filterationArea: ElementRef;
    @ViewChild('dataTableElement') dataTableElement: ElementRef;
    @ViewChild('dataTableActionsWrapperElement') dataTableActionsWrapperElement: ElementRef;
    currentLanguage: string;
    localizationSourceName = AppConsts.localization.defaultLocalizationSourceName;
    localization: LocalizationService;
    permission: PermissionCheckerService;
    feature: FeatureCheckerService;
    notify: NotifyService;
    setting: SettingService;
    message: MessageService;
    multiTenancy: AbpMultiTenancyService;
    appSession: AppSessionService;
    elementRef: ElementRef;
    isDeleted: boolean = false;

    constructor(injector: Injector) {
        //this.cfr = injector.get(ComponentFactoryResolver);
        this.renderer = injector.get(Renderer2);
        this.router = injector.get(Router);
        this.baseUrl = injector.get(API_BASE_URL);
        this.localization = injector.get(LocalizationService);
        this.currentLanguage = this.localization.currentLanguage.name;
        this.permission = injector.get(PermissionCheckerService);
        this.feature = injector.get(FeatureCheckerService);
        this.notify = injector.get(NotifyService);
        this.setting = injector.get(SettingService);
        this.message = injector.get(MessageService);
        this.multiTenancy = injector.get(AbpMultiTenancyService);
        this.appSession = injector.get(AppSessionService);
        this.elementRef = injector.get(ElementRef);
    }

    l(key: string, ...args: any[]): string {
        let localizedText = this.localization.localize(key, this.localizationSourceName);

        if (!localizedText) {
            localizedText = key;
        }

        if (!args || !args.length) {
            return localizedText;
        }

        args.unshift(localizedText);
        return abp.utils.formatString.apply(this, args);
    }

    getDateFormate(date): string {
      //  return moment(date).format('DD MMM YYYY');
        return this.convertDateFromHijri(date, 'D MMM YYYY', "en");
    }

    getTimeFormate(date): string {
       return moment(date).format('hh:mm A');
       // return this.convertDateFromHijri(date, 'D MMM YYYY', "en");
    }

    getDateTimeFormate(date): string {
        //return moment(date).format('DD MMM YYYY hh:mm A');
        return this.convertDateFromHijri(date, 'D MMM YYYY hh:mm A', "en");
    }
    convertDateFromHijri(date, format, lang): string {
        var year = +moment(date).locale("en").format('YYYY');
        if (year > 1900) {
            return moment(date).locale(lang).format(format) || '';
        } else if (year < 1500) {
            var f = momenthijri(date).locale("en").format('YYYY-M-D HH:mm:ss');
            var mh = momenthijri(f, 'iYYYY-iM-iD HH:mm:ss');
            var m = mh.locale(lang).format(format);
            return m;
        }
        return date;
    }
    isGranted(permissionName: string): boolean {
        return this.permission.isGranted(permissionName);
    }

    ngOnInit(): void {

    }

    initDataTable(initDataTableInput: InitDataTableInput): any[] {
        let colDefs: any[] = [];
        // auto target columns
        for (var i = 0; i < initDataTableInput.ColumnDefs.length; i++) {
            initDataTableInput.ColumnDefs[i].targets = i;
            let colDef = initDataTableInput.ColumnDefs[i];
            if (colDef.searchable) {
                let newColDef: any = {};
                newColDef.title = colDef.title;
                newColDef.name = colDef.name;
                if (colDef.visible == undefined) {
                    newColDef.visible = true;
                    newColDef.disabled = true;
                }
                else {
                    newColDef.visible = colDef.visible;
                    newColDef.disabled = false;
                }
                colDefs.push(newColDef);
            }
        }
        this.dtOptions = {
            lengthMenu: [
                [10, 20, 50, 1000000000],
                [10, 20, 50, this.l('Common.Any')] // change per page values here
            ],
            ajax: {
                url: this.baseUrl + initDataTableInput.DataTableSrc,
                headers: {
                    Authorization: 'Bearer ' + abp.auth.getToken()
                },
                type: "POST", // request type
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                timeout: 60000,
                data: data => this.getFilter(data)
            },
            stateSave: false, // save datatable state(pagination, sort, etc) in cookie. //Setting SaveDataTableState
            order: [// set first column as a default sort by asc
                [1, "asc"]
            ],
            //data: result.items,
            columnDefs: initDataTableInput.ColumnDefs,
            processing: false,
            serverSide: true,
            pagingType: 'bootstrap_extended',
            pageLength: 10,
            customLanguagePaginateSettings: {
                page: "1",
                pageOf: this.l("Common.DataTable.Paginate.PageOf")
            },
            language: {
                lengthMenu: this.l('Common.DataTable.LengthMenu'),
                info: "<span class='seperator'>|</span>" + this.l('Common.DataTable.Info'), // "Showing _START_ to _END_ of _TOTAL_ entries" "<span class='seperator'> | </span>"
                infoEmpty: this.l('Common.DataTable.InfoEmpty'),
                infoFiltered: this.l('Common.DataTable.InfoFiltered'),
                emptyTable: this.l('Common.DataTable.EmptyTable'),
                zeroRecords: this.l('Common.DataTable.ZeroRecords'),
                loadingRecords: this.l('Common.Loading'),
                paginate: {
                    previous: "<span>" + this.l('Common.DataTable.Paginate.Previous') + "</span>",
                    next: "<span>" + this.l('Common.DataTable.Paginate.Next') + "</span>",
                    last: "<span>" + this.l('Common.DataTable.Paginate.Last') + "</span>",
                    first: "<span>" + this.l('Common.DataTable.Paginate.First') + "</span>"
                }
            },
            orderCellsTop: true,
            "dom": "<'row smaller-paging'<'col-md-12 col-sm-12'pli><'col-md-4 col-sm-12'<'table-group-actions pull-left'>>r><'table-scrollable't><'table-scrollable't><'row smaller-paging'<'col-md-12 col-sm-12'pli><'col-md-4 col-sm-12'>>", // datatable layout
            drawCallback: oSettings => this.drawCallback(oSettings),
            //rowCallback: (data, row, index) => {
            //    //let cmp = this.createComponent(this.tbody);
            //    //// set inputs..
            //    //cmp.instance.data = data.outerHTML;
            //    ////// set outputs..
            //    ////cmp.instance.clicked.subscribe(event => console.log(`clicked: ${event}`));
            //    //// all inputs/outputs set? add it to the DOM ..
            //    //this.tbody.insert(cmp.hostView);
            //    //    // Use the ComponentFactoryResolver to populate the <table-cell-component>?
            //},
            createdRow: (row, data, dataIndex) => {
                const self = this;
                let editDom: any = $(row).find(".edit");
                if (editDom != null && editDom != undefined) {
                    editDom.unbind('click');
                    editDom.bind('click', () => {
                        self.edit(data);
                    });
                }
                let deleteDom = $(row).find(".delete");
                deleteDom.unbind('click');
                deleteDom.bind('click', () => {
                    const action = deleteDom.attr('data-action');
                    const confirm = deleteDom.hasClass('confirm');
                    const id = [deleteDom.attr('data-id')];
                    const dataStatic = deleteDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });
                let activeDom = $(row).find(".active");
                activeDom.unbind('click');
                activeDom.bind('click', () => {
                    const action = activeDom.attr('data-action');
                    const confirm = activeDom.hasClass('confirm');
                    const id = [activeDom.attr('data-id')];
                    const dataStatic = activeDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });
             
                let deactivateDom = $(row).find(".deactivate");
                deactivateDom.unbind('click');
                deactivateDom.bind('click', () => {
                    const action = deactivateDom.attr('data-action');
                    const confirm = deactivateDom.hasClass('confirm');
                    const id = [deactivateDom.attr('data-id')];
                    const dataStatic = deactivateDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });
                let acceptDom = $(row).find(".accept");
                acceptDom.unbind('click');
                acceptDom.bind('click', () => {
                    const action = acceptDom.attr('data-action');
                    const confirm = acceptDom.hasClass('confirm');
                    const id = [acceptDom.attr('data-id')];
                    const dataStatic = acceptDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });
                let rejectDom = $(row).find(".reject");
                rejectDom.unbind('click');
                rejectDom.bind('click', () => {
                    const action = rejectDom.attr('data-action');
                    const confirm = rejectDom.hasClass('confirm');
                    const id = [rejectDom.attr('data-id')];
                    const dataStatic = rejectDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });
                let rejectResonDom: any = $(row).find(".rejectResonBalance");
                if (rejectResonDom != null && rejectResonDom != undefined) {
                    //const action = rejectResonDom.attr('data-action');
                    //const confirm = rejectResonDom.hasClass('confirm');
                    //const id = [rejectResonDom.attr('data-id')];
                    //const dataStatic = rejectDom.attr('data-static');

                    rejectResonDom.unbind('click');
                    rejectResonDom.bind('click', () => {
                        self.rejectResonBalance(data);

                     /*   this.sendSingleAction(action, dataStatic, confirm, id);*/
                    });
                }
                let publishDom = $(row).find(".publish");
                publishDom.unbind('click');
                publishDom.bind('click', () => {
                    const action = publishDom.attr('data-action');
                    const confirm = publishDom.hasClass('confirm');
                    const id = [publishDom.attr('data-id')];
                    const dataStatic = publishDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });
                let unPublishDom = $(row).find(".unPublish");
                unPublishDom.unbind('click');
                unPublishDom.bind('click', () => {
                    const action = unPublishDom.attr('data-action');
                    const confirm = unPublishDom.hasClass('confirm');
                    const id = [unPublishDom.attr('data-id')];
                    const dataStatic = unPublishDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });
                let approveDom = $(row).find(".approve");
                approveDom.unbind('click');
                approveDom.bind('click', () => {
                    const action = approveDom.attr('data-action');
                    const confirm = approveDom.hasClass('confirm');
                    const id = [approveDom.attr('data-id')];
                    const dataStatic = approveDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });
                let declineDom = $(row).find(".decline");
                declineDom.unbind('click');
                declineDom.bind('click', () => {
                    const action = declineDom.attr('data-action');
                    const confirm = declineDom.hasClass('confirm');
                    const id = [declineDom.attr('data-id')];
                    const dataStatic = declineDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });

                let enableDom = $(row).find(".enable");
                enableDom.unbind('click');
                enableDom.bind('click', () => {
                    const action = enableDom.attr('data-action');
                    const confirm = enableDom.hasClass('confirm');
                    const id = [enableDom.attr('data-id')];
                    const dataStatic = enableDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });
                let disableDom = $(row).find(".disable");
                disableDom.unbind('click');
                disableDom.bind('click', () => {
                    const action = disableDom.attr('data-action');
                    const confirm = disableDom.hasClass('confirm');
                    const id = [disableDom.attr('data-id')];
                    const dataStatic = disableDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });

                let exportDom = $(row).find(".export");
                exportDom.unbind('click');
                exportDom.bind('click', () => {
                    const action = exportDom.attr('data-action');
                    const confirm = exportDom.hasClass('confirm');
                    const id = [exportDom.attr('data-id')];
                    const dataStatic = exportDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });
                let resetPasswordDom = $(row).find(".resetPassword");
                resetPasswordDom.unbind('click');
                resetPasswordDom.bind('click', () => {
                    const action = resetPasswordDom.attr('data-action');
                    const confirm = resetPasswordDom.hasClass('confirm');
                    const id = [resetPasswordDom.attr('data-id')];
                    const dataStatic = resetPasswordDom.attr('data-static');
                    this.sendSingleAction(action, dataStatic, confirm, id);
                });
                /* gotoUrl */
                let urlParms = [];
                let gotoUrlDom = $(row).find(".gotoUrl");
                let params = gotoUrlDom.attr("routerLink-params");
                if (params && params != null) {
                    JSON.parse(params, (key, value) => {
                        urlParms.push(value);
                    });
                    gotoUrlDom.unbind('click');
                    gotoUrlDom.bind('click', () => {
                        self.gotoUrl.apply(this, urlParms);
                    });
                }
            },
            initComplete: (settings, json) => {
                // build table group actions panel
                if ($('.table-actions-wrapper').length === 1) {
                    $('.table-group-actions').html($('.table-actions-wrapper').html()); // place the panel inside the wrapper
                    $('.table-actions-wrapper').hide(); // remove the template container
                }
                this.renderer.listen(this.dataTableActionsWrapperElement.nativeElement, 'click', (event) => {
                    if (event.target.attributes && event.target.attributes['role']
                        && event.target.attributes['role'].value == 'reset-table') {
                        this.rerender();
                    }
                });
            }
        };
        this.initialParams = initDataTableInput.InitialParams;
        setTimeout(() => {
            this.dtTrigger.next();
        });
        return colDefs;
    }

    sendSingleAction(action, dataStatic, confirm, id) {
        if (action === 'Delete' && dataStatic == "true") {
            abp.message.error(this.l('Common.Error.StaticData'));
            return;
        }
        if (confirm) {
            const message = this.l('Common.Message.Confirm.' + action);
            abp.message.confirm(message, '', (result: boolean) => {
                if (result) {
                    this.setAjaxParam("actionType", "SingleAction");
                    this.setAjaxParam("action", action);
                    this.setAjaxParam("ids", id);
                    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                        dtInstance.ajax.reload();
                    });
                }
            }
            );
        }
        else {
            this.setAjaxParam("actionType", "SingleAction");
            this.setAjaxParam("action", action);
            this.setAjaxParam("ids", id);
            this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.ajax.reload();
            });
        }
    }

    changeColumnView(colDef): void {
        colDef.visible = !colDef.visible;
        this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
            const column = dtInstance.column(colDef.name + ':name');
            if (column != undefined) {
                column.visible(colDef.visible);
            }
        });
    };
    rerender(): void {
        this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
            // // Destroy the table first
            // dtInstance.destroy();
            // // Call the dtTrigger to rerender again
            // setTimeout(() => {
            //     this.dtTrigger.next();
            // });
            setTimeout(() => {
                dtInstance.ajax.reload();
            });
        });
    }
    private getFilter(data: {}): string {
        abp.ui.setBusy(this.dataTableElement.nativeElement);
        //set ajaxParams
        if (this.ajaxParams != null && this.ajaxParams.length > 0) {
            for (var i = 0; i < this.ajaxParams.length; i++) {
                if (this.ajaxParams[i].key.indexOf('filterField.') > -1)
                    data[this.ajaxParams[i].key.substr(12)] = this.ajaxParams[i].value;
                else
                    data[this.ajaxParams[i].key] = this.ajaxParams[i].value;
            }
        }
        //set initParams
        if (this.initialParams != null && this.initialParams.length > 0) {
            for (var i = 0; i < this.initialParams.length; i++) {
                data[this.initialParams[i].key] = this.initialParams[i].value;
            }
        }
        return JSON.stringify(data);
    }
    /* DataTable Helper Methods */
    /* start events */
    // handle reset table
    resetTable(): void {
        this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
            //Reset sort
            dtInstance.order([[1, 'asc']]);
            //clear state
            dtInstance.state.clear();
            // reset table
            this.resetFilter();
        });
    }
    groupedActions(action) {
        let ids = [];
        $(this.dataTableElement.nativeElement).find('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked').each((ind: number, elem: Element) => {
            let id = $(elem).attr('id');
            ids.push(id);
        });
        this.setAjaxParam("actionType", "GroupAction");
        this.setAjaxParam("action", action);
        this.setAjaxParam("ids", ids);
        this.rerender();
    }
    // handle filter submit button click
    submitFilter(): void {
        // get all typeable inputs
        $(this.filterationArea.nativeElement).find('textarea.form-filter, select.form-filter, input.form-filter:not([type="checkbox"],[type="radio"])').each((ind: number, elem: Element) => {
            //check if input is datetime
            if ($(elem).hasClass('jqcalendar') && $(elem).val() != undefined) {
                //check if displayed in hijri
                //if (Utilities.currentLang() == 'ar' && Utilities.isDisplayHijri()) {
                //    //convert to georgian
                //    the.setAjaxParam('filterField.' + $(this).attr("name"), Utilities.convertToGregorian($(this).val()));
                //    return;
                //}
            }
            this.setAjaxParam("filterField." + $(elem).attr("name"), $(elem).val());
        });

        // get all checkboxes and radio
        $(this.filterationArea.nativeElement).find('input.form-filter[type="radio"],input.form-filter[type="checkbox"]').each((ind: number, elem: Element) => {
            this.setAjaxParam('filterField.' + $(elem).attr("name"), $(elem).is(':checked'));
        });

        // set additional lang filter
        this.setAjaxParam('filterField.lang', abp.localization.currentLanguage.name);


        this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
            dtInstance.ajax.reload();
        });
    }
    checkAll(): void {
        //let checked = $('.group-checkable').is(":checked");
        // get all checkboxes and radio
        $(this.dataTableElement.nativeElement).find('tbody > tr > td:nth-child(1) input[type="checkbox"]:not([disabled])').each((ind: number, elem: Element) => {
            /*if (!checked)
                $(elem).removeAttr('checked');
            else
                $(elem).attr('checked', 'checked');*/
            $(elem).click();
        });
        //this.countSelectedRecords();
    }
    checkItem(): void {
        const selected = $('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked', this.dataTableElement.nativeElement).length;
        const displayed = $('tbody > tr > td:nth-child(1) input[type="checkbox"]', this.dataTableElement.nativeElement).length;
        $('.group-checkable', this.dataTableElement.nativeElement).prop("checked", selected === displayed);
        this.countSelectedRecords();
    }
    /* end events */

    /* start private methods */

    countSelectedRecords(): void {
        var selected = this.getSelectedRowsCount();
        var text = this.l('Common.DataTable.SelectedCount');
        if (selected > 0) {
            $(this.dataTableActionsWrapperElement.nativeElement).find('.group-action-input').show();
            $(this.dataTableActionsWrapperElement.nativeElement).find('.table-group-actions > label').show();
            $(this.dataTableActionsWrapperElement.nativeElement).find('.table-group-actions > span').text(text.replace("_TOTAL_", selected.toString()));
        } else {
            $(this.dataTableActionsWrapperElement.nativeElement).find('.group-action-input').hide();
            $(this.dataTableActionsWrapperElement.nativeElement).find('.table-group-actions > label').hide();
            $(this.dataTableActionsWrapperElement.nativeElement).find('.table-group-actions > span').text('');
        }
    };
    setAjaxParam(key: string, value: any): void {
        let existsParam = this.ajaxParams.find(x => x.key == name);
        if (existsParam != null && existsParam != undefined)
            existsParam.value = value;
        else
            this.ajaxParams.push({ key: key, value: value });
    }
    ShowDeleted(isDeleted) {
        this.isDeleted = isDeleted;
        this.setAjaxParam("isDeleted", isDeleted);
        this.rerender();
    }
    addAjaxParam(name, value): void {
        this.ajaxParams.push({ key: name, value: value });
    }
    clearAjaxParams(): void {
        this.ajaxParams = [];
    }
    private resetFilter(): void {
        $(this.filterationArea.nativeElement).find('textarea.form-filter, select.form-filter, input.form-filter').each((ind: number, elem: Element) => {
            $(elem).val("");
        });

        $(this.filterationArea.nativeElement).find('input.form-filter[type="checkbox"]').each((ind: number, elem: Element) => {
            $(elem).attr("checked", "false");
        });

        this.clearAjaxParams();
        //the.addAjaxParam("action", tableOptions.filterCancelAction);
        this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
            dtInstance.ajax.reload();
        });
    }
    private getSelectedRowsCount(): number {
        return $(this.dataTableElement.nativeElement).find('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked').length;
    }
    private getSelectedRows(): any[] {
        let rows = [];
        $(this.filterationArea.nativeElement).find('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked').each((ind: number, elem: Element) => {
            rows.push($(elem).val());
        });
        return rows;
    }
    private drawCallback(oSettings: any): void { // run some code on table redraw
        //if (tableInitialized === false) { // check if table has been initialized
        //    tableInitialized = true; // set table initialized
        //    table.show(); // display table
        //}

        $(this.dataTableElement.nativeElement).find('input[type="checkbox"]').removeAttr('checked');
        $(this.dataTableElement.nativeElement).find('tbody > tr > td:nth-child(1) input[type="checkbox"]:not([disabled])').on('click', () => {
            this.checkItem();
        });
        this.countSelectedRecords(); // reset selected records indicator

        //// callback for ajax data load
        //if (tableOptions.onDataLoad) {
        //    tableOptions.onDataLoad.call(undefined, the);
        //}
        this.stockValue = oSettings.json.result.stockValue;
        this.totalRecords = oSettings._iRecordsTotal;
        this.filteredCount = oSettings.json.result.iTotalDisplayRecords;
        ////additionalData
        //this.additionalData = oSettings.json.result.additionalData;
        ////aaData For Excel && PDF
        //this.aaData = oSettings.json.result.aaData;
        //Metronic.initUniform($('input[type="checkbox"]', table)); // reinitialize uniform checkboxes on each table reload
        //$.uniform.update($('.group-checkable', table));
        abp.ui.clearBusy(this.dataTableElement.nativeElement);
    };
    /* end private methods */
    protected abstract edit?(obj: object): void;
    protected abstract delete?(obj: object): void; 
    protected abstract rejectResonBalance?(obj: object): void;
    gotoUrl(...params: any[]): void {
        //navigateByUrl
        this.router.navigate([...params]);
    };
}

export class InitDataTableInput {
    InitialParams: any[] = [];
    ColumnDefs: DataTables.ColumnDefsSettings[] = [];
    DataTableSrc: string = "";
}
interface ICustomColDef {
    name: string;
    visible: Boolean;
    disabled: Boolean;
}
