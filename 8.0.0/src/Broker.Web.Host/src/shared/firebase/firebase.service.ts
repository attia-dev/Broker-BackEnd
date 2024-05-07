import { Injectable, Injector } from '@angular/core';
import { AngularFireDatabase } from '@angular/fire/database';
import { AngularFireMessaging } from '@angular/fire/messaging';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppSessionService } from '@shared/session/app-session.service';
import * as moment from 'moment';
import { FirebaseMessageDto } from './FirebaseMessageDto';
import { NotifyService } from 'abp-ng2-module';
  
@Injectable({
    providedIn: 'root'
})
export class FirebaseService {

    appSession: AppSessionService;
    notify: NotifyService;
    currentMessage = new BehaviorSubject(null);

    constructor(injector: Injector, private db: AngularFireDatabase, private fcm: AngularFireMessaging) { 
        this.appSession = injector.get(AppSessionService);
        this.notify = injector.get(NotifyService);
    }

    getChat(bookingId): Observable<any> {
        debugger;
        return this.db.list('chat/' + bookingId).valueChanges();
    }

    sendMessage(bookingId, message: string, userIdTo: number) {
        const bookingRef = this.db.list('chat/' + bookingId);
        bookingRef.push(this.buildChatMessage(message, userIdTo));
    }

    private buildChatMessage(message: string, userIdTo: number): FirebaseMessageDto {
        debugger;
        var newMessage = new FirebaseMessageDto();
        newMessage.text = message;
        newMessage.dateTime = moment().format('YYYY-MM-DD HH:mm:ss');
        newMessage.userIdFrom = this.appSession.userId;
        newMessage.userIdTo = userIdTo??0;
        return newMessage;
    }

    deleteBookingChat(bookingId) {
        const bookingRef = this.db.list('chat/' + bookingId);
        bookingRef.remove();
    }

    requestPermission() {
        this.fcm.requestToken.subscribe((token) => {
            console.log('Permission granted! Save to the server!', token);
        }, (error) => {
            console.log(error);
        })
    }
    
    receiveMessage() {
        this.fcm.onMessage((payload) => {
            console.log('Message received. ', payload);
            this.notify.info(payload);
            this.currentMessage.next(payload);
        });
    }

}
