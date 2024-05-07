import { Component, Injector, OnInit } from '@angular/core';
import { NbMenuItem } from '@nebular/theme';
import { AppComponentBase } from '@shared/app-component-base';

import { MENU_ITEMS } from './pages-menu';

@Component({
    selector: 'ngx-pages',
    styleUrls: ['pages.component.scss'],
    template: `
    <ngx-one-column-layout>
      <nb-menu [items]="menu"></nb-menu>
      <router-outlet></router-outlet>
    </ngx-one-column-layout>
  `,
})
export class PagesComponent extends AppComponentBase implements OnInit {

    menu: NbMenuItem[] = [];

    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit(): void {
        this.menu = this.localizeItemTile(MENU_ITEMS);
    }

    isMenuItemVisible(item: NbMenuItem): boolean {
        if (!item || !item.fragment) {
            return true;
        }
        return this.permission.isGranted(item.fragment);
    }

    localizeItemTile(list: NbMenuItem[]): NbMenuItem[] {
        list.forEach((v, i) => {
            v.title = this.l(v.title);
            v.hidden = !this.isMenuItemVisible(v);
            if (v.children) {
                v.children = this.localizeItemTile(v.children);
            }
        });
        return list;
    }

}
