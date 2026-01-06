
# Santa Clash

Santa Clash est un petit jeu 2D développé en **C# avec MonoGame**.  
Deux joueurs coopèrent pour protéger Santa contre des vagues d’ennemis.

---

## Principe du jeu

- Santa se trouve au centre de l’écran.
- Des ennemis apparaissent progressivement et se dirigent vers lui.
- Les joueurs doivent éliminer les ennemis avant qu’ils n’atteignent Santa.
- La partie se termine lorsque **Santa n’a plus de points de vie**.

---

## Joueurs et contrôles

### Joueur 1 (Manette)
- Déplacement : **D-pad**
- Tir : **Bouton A**

### Joueur 2 (Clavier)
- Déplacement : **W / A / S / D**
- Tir : **Espace**

---

## Règles du jeu

- Chaque ennemi éliminé rapporte **1 point** au joueur correspondant.
- La précision dépend du nombre de tirs réussis par rapport aux tirs effectués.
- Les vagues deviennent plus difficiles au fil du temps.
- Si un ennemi touche Santa, il lui inflige des dégâts.

---

## Écrans du jeu

- **Menu**  
  Appuyer sur **Entrée** (ou Start) pour commencer une partie.

- **En jeu**  
  Affichage :
  - Points de vie de Santa  
  - Scores des joueurs

- **Game Over**  
  - Affichage des scores finaux  
  - Appuyer sur **Entrée** pour revenir au menu

---

## Structure du projet (simplifiée)

```text
P_SantaClash/
├── Program.cs
├── Game1.cs
├── Enemy.cs
├── Player.cs
├── Santa.cs
├── Projectile.cs
├── WaveManager.cs
├── GameStateManager.cs
└── Content/
    └── wallpaper.png
