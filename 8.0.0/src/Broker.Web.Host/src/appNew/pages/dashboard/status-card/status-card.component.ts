import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'ngx-status-card',
    styleUrls: ['./status-card.component.scss'],
    template: `
    <nb-card (click)="goToUrl(link)">
      <div class="icon-container">
        <div class="icon status-{{ type }}">
          <ng-content></ng-content>
        </div>
      </div>
      <div class="details">
        <div class="title h6">{{ title }}</div>
        <div class="status paragraph-2">{{ count }}</div>
      </div>
    </nb-card>
  `,
})
export class StatusCardComponent {

    @Input() title: string;
    @Input() type:  string;
    @Input() link:  string;
    @Input() count: number;

    constructor(private router: Router) {
    }

    goToUrl(url) {
        this.router.navigate([url]);
    }

}
