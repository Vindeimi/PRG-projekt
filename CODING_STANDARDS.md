# 📜 Pravidla přispívání do projektu  

## 🔄 Práce s větvemi  
- Hlavní větev je `master`, každý má svou vlastní větev (`sam`, `honza`, `pavel`).  
- Po dokončení změn se dělá **Pull Request (PR)** do `master`.  

> [!WARNING]  
> Nepřidávejte nové změny přímo do `master` – vždy použijte svou větev a PR!  

> [!TIP]  
> Pokud pracujete na větší změně, používejte menší commity a pravidelně pushujte.  

---

## ✅ Code review  
- PR musí být schválen alespoň **jedním členem týmu**.  
- Kód musí být přehledný a odpovídat stylu projektu.  

> [!IMPORTANT]  
> Před odesláním PR si zkontrolujte:  
> ✔️ Kód odpovídá našemu stylu  
> ✔️ Změny jsou dobře zdokumentované  
> ✔️ Projekt se správně sestaví  

> [!TIP]  
> Pokud je PR větší, popište hlavní změny v popisu PR, ať se v něm ostatní rychle zorientují.  

---

## 📏 Styl kódu  
- Používáme vlastní **konvence názvů**:  
  - **Public globální proměnná** → `int BigNum`  
  - **Private globální proměnná** → `_bigNum`  
  - **Lokální proměnná** → `bigNum`  
  - **Funkce** → `BigNum()`  
  - **Konstanty** → `BIG_NUM`  
- Každá třída a metoda musí mít komentář (`///`).  
- Konzistentní struktura složek:  
  - **/Models** → Datové modely  
  - **/Views** → WPF XAML soubory  
  - **/ViewModels** → Logika aplikace (MVVM)  
  - **/Services** → API komunikace  

> [!WARNING]  
> Nepoužívejte hardcoded hodnoty – používejte **konstanty** nebo **konfigurační soubory**!  

> [!TIP]  
> Pokud si nejste jisti, jak napsat kód správně, inspirujte se existujícím kódem v repozitáři.  

---
