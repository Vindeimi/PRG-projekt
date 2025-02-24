# Práce s Gitem

## 📝 Úvod
Git je systém pro správu verzí, který umožňuje sledovat změny v kódu, spolupracovat s ostatními a spravovat různé verze projektu. Tento dokument popisuje základní příkazy a workflow pro práci s Gitem.

---

## 🔄 Základní workflow pro práci na vlastním úkolu

### 1. **Přepnutí na svou větev**
Nejdříve se ujistěte, že pracujete na své vlastní větvi. Můžete použít následující příkaz:
- `git checkout sam`  # Přepnutí na svou větev (nahraďte 'sam' svým jménem)

### 2. **Aktualizace z hlavní větve**
Předtím, než začnete pracovat na nových změnách, stáhněte nejnovější změny z hlavní větve `master`:
- `git pull origin master`  # Stažení nejnovější verze hlavní větve

### 3. **Provádění změn**
Nyní můžete začít pracovat na svých změnách. Jakmile dokončíte úpravy, přidejte soubory do stage:
- `git add .`  # Přidání všech změn do stage

### 4. **Commitování změn**
Po přidání změn do stage je čas vytvořit commit. Ujistěte se, že commit zpráva odpovídá konvencím:
- `git commit -m "[Feature] Přidána nová funkce"`  # Přizpůsobte zprávu svým změnám

### 5. **Odeslání změn na GitHub**
Jakmile máte hotové commity, můžete je odeslat na svůj repozitář na GitHub:
- `git push origin sam`  # Odeslání změn na svou větev

### 6. **Vytvoření Pull Requestu**
Po odeslání změn na GitHub vytvořte Pull Request (PR) do hlavní větve `master`. Otevřete repozitář na GitHubu a klikněte na tlačítko "Compare & pull request".

### 7. **Code review a schválení**
Po vytvoření PR počkejte na schválení od ostatních členů týmu. Všichni by měli zkontrolovat změny a poskytnout případnou zpětnou vazbu.

### 8. **Merge do `master`**
Jakmile je PR schváleno, můžete provést merge do hlavní větve `master`. To obvykle provádí osoba, která PR schválila.

### 9. **Údržba**
Po merge do `master` je dobré aktualizovat svou větev, abyste měli nejnovější změny. Použijte:
- `git checkout sam`  # Přepnutí na svou větev
- `git pull origin master`  # Aktualizace z hlavní větve

---

## 📚 Základní příkazy Gitu

- **`git status`** – Zobrazí aktuální stav vaší pracovní kopie (změny, které byly provedeny).
- **`git log`** – Zobrazí historii commitů.
- **`git branch`** – Zobrazí seznam větví v repozitáři.
- **`git checkout -b nová_větev`** – Vytvoří novou větev a přepne se na ni.
- **`git merge název_větve`** – Sloučí změny z vybrané větve do aktuální větve.
- **`git pull origin jméno_větve`** – Stáhne a sloučí změny z vybrané větve na GitHubu do vaší aktuální větve.

---

## ⚠️ Pozor na konflikty

Při sloučení větví může dojít ke konfliktům, pokud byly ve stejných místech kódu provedeny různé změny. V takovém případě Git označí konfliktní soubory a budete muset ručně vyřešit konflikty před dokončením merge.
