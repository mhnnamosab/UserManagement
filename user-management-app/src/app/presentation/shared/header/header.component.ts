import { DOCUMENT } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LocalStorageService } from '../../../core/utils/localstorage/local-storage.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {
  languages = [
    { code: 'en', label: 'English', icon: '🇺🇸' },
    { code: 'ar', label: 'العربية', icon: 'uae' },
  ];

  selectedLanguage = this.languages[0];

  constructor(private translate: TranslateService,private localStorageSerice:LocalStorageService,
    @Inject(DOCUMENT) private document: Document) {
    this.translate.setDefaultLang('en');
    const savedLang = this.localStorageSerice.getItem('language') || 'en';
    translate.use(savedLang);
    this.setDirection(savedLang);
  }

  changeLanguage(language: string): void {
    this.translate.use(language);
    this.setDirection(language);
    this.localStorageSerice.setItem('language', language)
    this.selectedLanguage = this.languages.find((lang) => lang.code === language)!;
  }

  private setDirection(lang: string): void {
    const htmlTag = this.document.documentElement;
    if (lang === 'ar') {
      htmlTag.setAttribute('dir', 'rtl');
      htmlTag.setAttribute('lang', 'ar');
    } else {
      htmlTag.setAttribute('dir', 'ltr');
      htmlTag.setAttribute('lang', 'en');
    }
  }
}