import {Injectable} from '@angular/core';
import {MessageService} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class CustomMessageService {
  constructor(private readonly messageService: MessageService, private readonly translateService: TranslateService) {
  }

  public pushSuccessMessage(title: string, content: string): void {
    const translatedTitle = this.translateService.instant(title);
    const translatedContent = this.translateService.instant(content);
    this.messageService.add({severity: 'success', summary: translatedTitle, detail: translatedContent});
  }

  public pushErrorMessage(title: string, content: string): void {
    const translatedTitle = this.translateService.instant(title);
    const translatedContent = this.translateService.instant(content);
    this.messageService.add({severity: 'error', summary: translatedTitle, detail: translatedContent});
  }

  public pushWarningMessage(title: string, content: string): void {
    const translatedTitle = this.translateService.instant(title);
    const translatedContent = this.translateService.instant(content);
    this.messageService.add({severity: 'warn', summary: translatedTitle, detail: translatedContent});
  }

  public pushInfoMessage(title: string, content: string): void {
    const translatedTitle = this.translateService.instant(title);
    const translatedContent = this.translateService.instant(content);
    this.messageService.add({severity: 'info', summary: translatedTitle, detail: translatedContent});
  }
}
