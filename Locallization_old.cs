using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using BepInEx;
using LocalizationManager;
using UnityEngine;

namespace EpicMMOSystem;

public class Localizationold
{
    private string defaultFileName = "eng_emmosLocalization.txt";
    private Dictionary<string, string> _dictionary = new ();

    public Localizationold()
    {
 
    var currentLanguage = global::Localization.instance.GetSelectedLanguage();
    if (currentLanguage == "Russian")
    {
        RusLocalization();
    }
    else if (currentLanguage == "English")
    {
        EngLocalization();
    }
    else if (currentLanguage == "Spanish")
    {
        SpanLocalization();
    }
    else if (currentLanguage == "German")
    {
        GermanLocalization();
    }
    else if (currentLanguage == "Chinese")
    {
        ChineseLocalization();
    }
    else if (currentLanguage == "Portuguese")
    {
        PortugueseLocalization();
    }
    else if (currentLanguage == "Swedish")
    {
        SwedishLocalization();
    }
    else if (currentLanguage == "French")
    {
        FrenchLocalization();
    }
    else
    {
        var fileName = $"Custom_EpicMMOSystem_Localization.txt";
        var basePath = Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName, fileName);
        if (File.Exists(basePath))
        {
            ReadLocalization(basePath);
            return;
        }
        CreateLocalizationFile();
    }
    
}

    private void ReadLocalization(string path)
    {
        var lines = File.ReadAllLines(path);
        EngLocalization();
        bool update = _dictionary.Count > lines.Length;
        foreach (var line in lines)
        {
            var pair = line.Split('=');
            var text = pair[1].Replace('*', '\n');
            _dictionary[pair[0].Trim()] = text.TrimStart();
        }
        if (update)
        {
            List<string> list = new List<string>();
            foreach (var pair in _dictionary)
            {
                list.Add($"{pair.Key} = {pair.Value}");
            }
            File.WriteAllLines(path, list);
        }
    }

    private void CreateLocalizationFile()
    {
        EngLocalization();
        List<string> list = new List<string>();
        foreach (var pair in _dictionary)
        {
            list.Add($"{pair.Key} = {pair.Value}");
        }
        
        var mmofolder = Path.Combine(Paths.ConfigPath, EpicMMOSystem.ModName);
        if (!Directory.Exists(mmofolder))
        {
            Directory.CreateDirectory(mmofolder);
        }

        File.WriteAllLines(Path.Combine(Paths.ConfigPath, EpicMMOSystem.ModName, defaultFileName), list);
    }

    private void RusLocalization()
    {
        _dictionary.Add("$attributes", "Параметры");
        _dictionary.Add("$parameter_strength", "Сила");
        _dictionary.Add("$parameter_intellect", "Интеллект");
        _dictionary.Add("$free_points", "Доступно очков");
        _dictionary.Add("$level", "Уровень");
        _dictionary.Add("$lvl", "Ур.");
        _dictionary.Add("$exp", "Опыт");
        _dictionary.Add("$cancel", "Отмена");
        _dictionary.Add("$apply", "Принять");
        _dictionary.Add("$reset_parameters", "Сбросить параметры");
        _dictionary.Add("$no", "Нет");
        _dictionary.Add("$yes", "Да");
        _dictionary.Add("$get_exp", "Получено опыта");
        _dictionary.Add("$reset_point_text", "Вы действительно хотите сбросить все поинты за {0} {1}?");
        //Parameter
        _dictionary.Add("$physic_damage", "Ув. физ. урона");
        _dictionary.Add("$add_weight", "Ув. переносимого веса");
        _dictionary.Add("$speed_attack", "Расход вын. на атаку");
        _dictionary.Add("$reduced_stamina", "Расход вын. (бег, прыжок)");
        _dictionary.Add("$magic_damage", "Ув. маг. урона");
        _dictionary.Add("$magic_armor", "Ув. маг. защиты");
        _dictionary.Add("$add_hp", "Ув. здоровья");
        _dictionary.Add("$add_stamina", "Ув. выносливости");
        _dictionary.Add("$physic_armor", "Ув. физ. защиты");
        _dictionary.Add("$reduced_stamina_block", "Расход вын. на блок");
        _dictionary.Add("$regen_hp", "Регенерация здоровья");
        _dictionary.Add("$damage", "Урон");
        _dictionary.Add("$armor", "Защита");
        _dictionary.Add("$survival", "Выживание");

        _dictionary.Add("$regen_eitr", "регенерация Eitr");
        _dictionary.Add("$stamina_reg", "Регенерация выносливости"); 
        _dictionary.Add("$add_eitr", "Повышение Eitr");

        // new/changed Params 1.7.0
        _dictionary.Add("$parameter_agility", "Ловкость");
        _dictionary.Add("$parameter_body", "Выносливость");
        _dictionary.Add("$parameter_vigour", "Энергия");
        _dictionary.Add("$parameter_special", "Специализация");
        _dictionary.Add("$specialother", "Особенный");//divheader
        _dictionary.Add("$attack_speed", "Скорость атаки");
        _dictionary.Add("$attack_stamina", "Расход выносливости атаки");
        _dictionary.Add("$crtcDmgMulti", "Множитель критического урона");
        _dictionary.Add("$mining_speed", "Увеличение урона от добычи полезных ископаемых");
        _dictionary.Add("$piece_health", "Увеличение здоровья куска");
        _dictionary.Add("$tree_cutting", "Увеличение урона дерева");
        _dictionary.Add("$crit_chance", "Увеличение шанса критического удара");
        //Friends list
        _dictionary.Add("$notify", "<color=#00E6FF>Оповещение</color>");
        _dictionary.Add("$friends_list", "Список друзей");
        _dictionary.Add("$send", "Отправить");
        _dictionary.Add("$invited", "Приглашения");
        _dictionary.Add("$friends", "Друзья");
        _dictionary.Add("$online", "В игре");
        _dictionary.Add("$offline", "Нет в игре");
        _dictionary.Add("$not_found", "Игрок {0} не найден.");
        _dictionary.Add("$send_invite", "Игроку {0}, отправлен запрос в друзья.");
        _dictionary.Add("$get_invite", "Получен запрос в друзья от {0}.");
        _dictionary.Add("$accept_invite", "Игрок {0}, принял запрос в друзья.");
        _dictionary.Add("$cancel_invite", "Игрок {0}, отменил запрос в друзья.");
        //Terminal
        _dictionary.Add("$terminal_set_level", "Вы получили {0} уровень");
        _dictionary.Add("$terminal_reset_points", "Ваши очки характеристик были сброшены");

        _dictionary.Add("$strength_tooltip", "<size=20>Сила усилит следующие эффекты:</size> \n" +
        "<color=yellow>Увеличение физического урона</color> \n" +
        "<color=blue>Увеличение грузоподъемности</color> \n" +
        "<color=green>Снижение расхода выносливости при блокировании</color> \n" +
        "<color=red>Увеличение критического урона при критических попаданиях</color>");

        _dictionary.Add("$dexterity_tooltip", "<size=20>Ловкость усилит следующие эффекты:</size> \n" +
        "<color=red>Увеличение скорости атаки (кроме луков)</color> \n" +
        "<color=yellow>Снижение расхода выносливости при атаке</color> \n" +
        "<color=green>Снижение расхода выносливости при беге/прыжках</color>");

        _dictionary.Add("$intelect_tooltip", "<size=20>Интеллект усилит следующие эффекты:</size> \n" +
        "<color=green>Увеличение урона от всех элементов</color> \n" +
        "<color=red>Увеличение базового количества Эйтра (после получения Эйтра)</color> \n" +
        "<color=red>Увеличение регенерации Эйтра</color>");

        _dictionary.Add("$endurance_tooltip", "<size=20>Стойкость усилит следующие эффекты:</size> \n" +
        "<color=yellow>Увеличение запаса выносливости</color> \n" +
        "<color=yellow>Увеличение регенерации выносливости</color> \n" +
        "<color=green>Снижение получаемого физического урона</color>");

        _dictionary.Add("$vigour_tooltip", "<size=20>Сила жизни усилит следующие эффекты:</size> \n" +
        "<color=red>Увеличение запаса здоровья</color> \n" +
        "<color=yellow>Регенерация здоровья</color> \n" +
        "<color=green>Снижение получаемого элементального урона</color>");

        _dictionary.Add("$special_tooltip", "<size=20>Специальные навыки усилит следующие эффекты:</size> \n" +
        "<color=red>Увеличение шанса критической атаки</color> \n" +
        "<color=blue>Увеличение урона при добыче</color> \n" +
        "<color=blue>Увеличение прочности конструкций</color> \n" +
        "<color=green>Увеличение урона при рубке деревьев</color>");
    }
    private void EngLocalization()
    {
        _dictionary.Add("$attributes", "Attributes");
        _dictionary.Add("$parameter_strength", "Strength");
        _dictionary.Add("$parameter_intellect", "Intellect");
        _dictionary.Add("$free_points", "Available points");
        _dictionary.Add("$level", "Level");
        _dictionary.Add("$lvl", "Lvl.");
        _dictionary.Add("$exp", "Experience");
        _dictionary.Add("$cancel", "Cancel");
        _dictionary.Add("$apply", "Accept");
        _dictionary.Add("$reset_parameters", "Reset points");
        _dictionary.Add("$no", "No");
        _dictionary.Add("$yes", "Yes");
        _dictionary.Add("$get_exp", "Experience received");
        _dictionary.Add("$reset_point_text", "Do you really want to drop all the points for {0} {1}?");
        //Parameter
        _dictionary.Add("$physic_damage", "Physical Damage");
        _dictionary.Add("$add_weight", "Carry weight");     
        _dictionary.Add("$reduced_stamina", "Stamina consumption (running, jumping)");
        _dictionary.Add("$magic_damage", "Elemental damage");
        _dictionary.Add("$magic_armor", "Elemental reduced");
        _dictionary.Add("$add_hp", "Health increase");
        _dictionary.Add("$add_stamina", "Stamina increase");
        _dictionary.Add("$physic_armor", "Physical reduced");
        _dictionary.Add("$reduced_stamina_block", "Block stamina consumption");
        _dictionary.Add("$regen_hp", "Health regeneration");
        _dictionary.Add("$damage", "Damage");
        _dictionary.Add("$armor", "Armor");
        _dictionary.Add("$survival", "Survival");
        _dictionary.Add("$regen_eitr", "Eitr regeneration");
        _dictionary.Add("$stamina_reg", "Stamina regeneration");
        _dictionary.Add("$add_eitr", "Eitr Increase");
        // new/changed Params 1.7.0
        _dictionary.Add("$parameter_agility", "Dexterity");
        _dictionary.Add("$parameter_body", "Endurance");
        _dictionary.Add("$parameter_vigour", "Vigour");
        _dictionary.Add("$parameter_special", "Specializing");
        _dictionary.Add("$specialother", "Special");//divheader
        _dictionary.Add("$attack_speed", "Attack speed");
        _dictionary.Add("$attack_stamina", "Attack stamina consumption");
        _dictionary.Add("$crtcDmgMulti", "Critical Damage Multiplier");
        _dictionary.Add("$mining_speed", "Mining Damage increase");
        _dictionary.Add("$piece_health", "Piece Health increase");
        _dictionary.Add("$tree_cutting", "Tree Damage increase");
        _dictionary.Add("$crit_chance", "Critical Chance increase");

        //Friends list
        _dictionary.Add("$notify", "<color=#00E6FF>Alert</color>");
        _dictionary.Add("$friends_list", "Friends list");
        _dictionary.Add("$send", "Send");
        _dictionary.Add("$invited", "Invitations");
        _dictionary.Add("$friends", "Friends");
        _dictionary.Add("$online", "Online");
        _dictionary.Add("$offline", "Offline");
        _dictionary.Add("$not_found", "Player {0} is not found.");
        _dictionary.Add("$send_invite", "A friend request has been sent to player {0}.");
        _dictionary.Add("$get_invite", "Received a friend request from {0}.");
        _dictionary.Add("$accept_invite", "Player {0}, accepted the friend request.");
        _dictionary.Add("$cancel_invite", "Player {0}, canceled his friend request.");
        //Terminal
        _dictionary.Add("$terminal_set_level", "You got {0} level");
        _dictionary.Add("$terminal_reset_points", "Your attributes points have been reset");


        _dictionary.Add("$strength_tooltip", "<size=20>Strength will enhance:</size> \n" +
                            "<color=yellow> Increase Physical Damage </color> \n" +
                            "<color=blue> Increase Carry Weight </color> \n" +
                            "<color=green> Decrease Block Stamina Consumption </color> \n" +
                            "<color=red> Increase Critical Damage when crit hits </color>");

        _dictionary.Add("$dexterity_tooltip", "<size=20>Dexterity will enhance:</size> \n" +
                            "<color=red> Increase Attack Speed (not bows)</color> \n" +
                            "<color=yellow> Decreased Attack Stamina Consumption </color> \n" +
                            "<color=green> Decreased Running/Jumping Stamina Consumption</color> ");


        _dictionary.Add("$intelect_tooltip", "<size=20>Intelligence will enhance:</size> \n" +
                            "<color=green> Increase all Elemental Damage </color>\n" +
                            "<color=red> Increase base Eitr amount (once you have eitr)</color> \n" +
                            "<color=red> Increase Eitr Regeneration</color> ");


        _dictionary.Add("$endurance_tooltip", "<size=20>Endurance will enhance:</size> \n" +
                            "<color=yellow> Increase Stamina amount</color>\n" +
                            "<color=yellow> Increase Stamina Regeneration </color> \n" +
                            "<color=green> Reduce Physical Damage Taken</color> ");


        _dictionary.Add("$vigour_tooltip", "<size=20>Vigour will enhance:</size> \n" +
                            "<color=red> Increase HP amount</color>\n" +
                            "<color=yellow> Health Regeneration </color> \n" +
                            "<color=green> Reduce Elemental Damage Taken</color> ");


        _dictionary.Add("$special_tooltip", "<size=20>Special will enhance:</size> \n" +
                            "<color=red> Increase Critical Attack Chance</color> \n" +
                            "<color=blue> Increase Mining Damage </color> \n" +
                            "<color=blue> Increase construction piece's health </color> \n" +
                            "<color=green> Increase Tree Cutting Damage</color>");
    }

    private void FrenchLocalization()
    {
        _dictionary.Add("$attributes", "Attributs");
        _dictionary.Add("$parameter_strength", "Force");
        _dictionary.Add("$parameter_intellect", "Intellect");
        _dictionary.Add("$free_points", "Points disponibles");
        _dictionary.Add("$level", "Niveau");
        _dictionary.Add("$lvl", "Niv.");
        _dictionary.Add("$exp", "Expérience");
        _dictionary.Add("$cancel", "Annuler");
        _dictionary.Add("$apply", "Accepter");
        _dictionary.Add("$reset_parameters", "Réinitialiser les points");
        _dictionary.Add("$no", "Non");
        _dictionary.Add("$yes", "Oui");
        _dictionary.Add("$get_exp", "Expérience obtenue");
        _dictionary.Add("$reset_point_text", "Voulez-vous vraiment supprimer tous les points pour {0} {1} ?");
        // Paramètre
        _dictionary.Add("$physic_damage", "Dommages Physiques");
        _dictionary.Add("$add_weight", "Capacité de Port");
        _dictionary.Add("$reduced_stamina", "Consommation de Stamina (course, saut)");
        _dictionary.Add("$magic_damage", "Dommages Élémentaires");
        _dictionary.Add("$magic_armor", "Réduction des Dommages Élémentaires");
        _dictionary.Add("$add_hp", "Augmentation des Points de Vie");
        _dictionary.Add("$add_stamina", "Augmentation de la Stamina");
        _dictionary.Add("$physic_armor", "Réduction des Dommages Physiques");
        _dictionary.Add("$reduced_stamina_block", "Consommation de Stamina lors du Blocage");
        _dictionary.Add("$regen_hp", "Régénération des Points de Vie");
        _dictionary.Add("$damage", "Dommages");
        _dictionary.Add("$armor", "Armure");
        _dictionary.Add("$survival", "Survie");
        _dictionary.Add("$regen_eitr", "Régénération d'Eitr");
        _dictionary.Add("$stamina_reg", "Régénération de la Stamina");
        _dictionary.Add("$add_eitr", "Augmentation d'Eitr");
        // Nouveaux/Modifiés Paramètres 1.7.0
        _dictionary.Add("$parameter_agility", "Agilité");
        _dictionary.Add("$parameter_body", "Endurance");
        _dictionary.Add("$parameter_vigour", "Vigueur");
        _dictionary.Add("$parameter_special", "Spécialisation");
        _dictionary.Add("$specialother", "Spécialisation"); // divheader
        _dictionary.Add("$attack_speed", "Vitesse d'Attaque");
        _dictionary.Add("$attack_stamina", "Consommation de Stamina lors de l'Attaque");
        _dictionary.Add("$crtcDmgMulti", "Multiplicateur de Dommages Critiques");
        _dictionary.Add("$mining_speed", "Augmentation des Dommages lors de l'Extraction");
        _dictionary.Add("$piece_health", "Augmentation des Points de Vie des Pièces");
        _dictionary.Add("$tree_cutting", "Augmentation des Dommages lors de la Coupe d'Arbres");
        _dictionary.Add("$crit_chance", "Augmentation des Chances de Critique");

        // Liste d'Amis
        _dictionary.Add("$notify", "<color=#00E6FF>Alerte</color>");
        _dictionary.Add("$friends_list", "Liste d'Amis");
        _dictionary.Add("$send", "Envoyer");
        _dictionary.Add("$invited", "Invitations");
        _dictionary.Add("$friends", "Amis");
        _dictionary.Add("$online", "En Ligne");
        _dictionary.Add("$offline", "Hors Ligne");
        _dictionary.Add("$not_found", "Le joueur {0} n'est pas trouvé.");
        _dictionary.Add("$send_invite", "Une demande d'ami a été envoyée au joueur {0}.");
        _dictionary.Add("$get_invite", "Vous avez reçu une demande d'ami de {0}.");
        _dictionary.Add("$accept_invite", "Le joueur {0} a accepté la demande d'ami.");
        _dictionary.Add("$cancel_invite", "Le joueur {0} a annulé sa demande d'ami.");
        // Terminal
        _dictionary.Add("$terminal_set_level", "Vous avez atteint le niveau {0}.");
        _dictionary.Add("$terminal_reset_points", "Vos points d'attributs ont été réinitialisés");

        _dictionary.Add("$strength_tooltip", "<size=20>La Force améliorera :</size> \n" +
            "<color=yellow> Augmentation des Dommages Physiques </color> \n" +
            "<color=blue> Augmentation de la Capacité de Port </color> \n" +
            "<color=green> Réduction de la Consommation de Stamina lors du Blocage </color> \n" +
            "<color=red> Augmentation des Dommages Critiques en cas de coups critiques </color>");

        _dictionary.Add("$dexterity_tooltip", "<size=20>L'Agilité améliorera :</size> \n" +
            "<color=red> Augmentation de la Vitesse d'Attaque (hors arcs) </color> \n" +
            "<color=yellow> Réduction de la Consommation de Stamina lors de l'Attaque </color> \n" +
            "<color=green> Réduction de la Consommation de Stamina lors de la Course/Saut </color>");

        _dictionary.Add("$intelect_tooltip", "<size=20>L'Intellect améliorera :</size> \n" +
            "<color=green> Augmentation de tous les Dommages Élémentaires </color> \n" +
            "<color=red> Augmentation de la Quantité d'Eitr de base (une fois que vous avez de l'Eitr) </color> \n" +
            "<color=red> Augmentation de la Régénération de l'Eitr </color>");

        _dictionary.Add("$endurance_tooltip", "<size=20>L'Endurance améliorera :</size> \n" +
            "<color=yellow> Augmentation de la Quantité de Stamina </color> \n" +
            "<color=yellow> Augmentation de la Régénération de Stamina </color> \n" +
            "<color=green> Réduction des Dommages Physiques Subis </color>");

        _dictionary.Add("$vigour_tooltip", "<size=20>La Vigueur améliorera :</size> \n" +
            "<color=red> Augmentation de la Quantité de Points de Vie </color> \n" +
            "<color=yellow> Régénération des Points de Vie </color> \n" +
            "<color=green> Réduction des Dommages Élémentaires Subis </color>");

        _dictionary.Add("$special_tooltip", "<size=20>La Spécialisation améliorera :</size> \n" +
            "<color=red> Augmentation des Chances d'Attaque Critique </color> \n" +
            "<color=blue> Augmentation des Dommages lors de l'Extraction </color> \n" +
            "<color=blue> Augmentation de la Durabilité des Pièces de Construction </color> \n" +
            "<color=green> Augmentation des Dommages lors de la Coupe d'Arbres </color>");


    }

    private void SwedishLocalization()
    {
        _dictionary.Add("$attributes", "Attribut");
        _dictionary.Add("$parameter_strength", "Styrka");
        _dictionary.Add("$parameter_intellect", "Intellekt");
        _dictionary.Add("$free_points", "Tillgängliga poäng");
        _dictionary.Add("$level", "Nivå");
        _dictionary.Add("$lvl", "Nivå.");
        _dictionary.Add("$exp", "Erfarenhet");
        _dictionary.Add("$cancel", "Avbryt");
        _dictionary.Add("$apply", "Godkänn");
        _dictionary.Add("$reset_parameters", "Återställ poäng");
        _dictionary.Add("$no", "Nej");
        _dictionary.Add("$yes", "Ja");
        _dictionary.Add("$get_exp", "Fått erfarenhet");
        _dictionary.Add("$reset_point_text", "Vill du verkligen ta bort alla poäng för {0} {1}?");
        //Parameter
        _dictionary.Add("$physic_damage", "Fysiskt Skada");
        _dictionary.Add("$add_weight", "Bärkapacitet");
        _dictionary.Add("$reduced_stamina", "Stamina-förbrukning (springa, hoppa)");
        _dictionary.Add("$magic_damage", "Elemental skada");
        _dictionary.Add("$magic_armor", "Elemental reducerad");
        _dictionary.Add("$add_hp", "Hälsa ökar");
        _dictionary.Add("$add_stamina", "Stamina ökar");
        _dictionary.Add("$physic_armor", "Fysisk reducerad");
        _dictionary.Add("$reduced_stamina_block", "Blockera stamina-förbrukning");
        _dictionary.Add("$regen_hp", "Hälsoregeneration");
        _dictionary.Add("$damage", "Skada");
        _dictionary.Add("$armor", "Rustning");
        _dictionary.Add("$survival", "Överlevnad");
        _dictionary.Add("$regen_eitr", "Eitr-regeneration");
        _dictionary.Add("$stamina_reg", "Stamina-regeneration");
        _dictionary.Add("$add_eitr", "Eitr-ökning");
        //Nya/ändrade parametrar 1.7.0
        _dictionary.Add("$parameter_agility", "Smidighet");
        _dictionary.Add("$parameter_body", "Uthållighet");
        _dictionary.Add("$parameter_vigour", "Kraft");
        _dictionary.Add("$parameter_special", "Specialisering");
        _dictionary.Add("$specialother", "Special");//divheader
        _dictionary.Add("$attack_speed", "Attackhastighet");
        _dictionary.Add("$attack_stamina", "Stamina-förbrukning vid attack");
        _dictionary.Add("$crtcDmgMulti", "Kritisk Skademultiplikator");
        _dictionary.Add("$mining_speed", "Ökad Skada vid Brytning");
        _dictionary.Add("$piece_health", "Ökad Hållbarhet för Delar");
        _dictionary.Add("$tree_cutting", "Ökad Skada vid Trädfällning");
        _dictionary.Add("$crit_chance", "Ökad Chans för Kritiska Träffar");

        //Vänlista
        _dictionary.Add("$notify", "<color=#00E6FF>Varning</color>");
        _dictionary.Add("$friends_list", "Vänlista");
        _dictionary.Add("$send", "Skicka");
        _dictionary.Add("$invited", "Inbjudningar");
        _dictionary.Add("$friends", "Vänner");
        _dictionary.Add("$online", "Online");
        _dictionary.Add("$offline", "Offline");
        _dictionary.Add("$not_found", "Spelare {0} hittades inte.");
        _dictionary.Add("$send_invite", "En vänförfrågan har skickats till spelare {0}.");
        _dictionary.Add("$get_invite", "Du har fått en vänförfrågan från {0}.");
        _dictionary.Add("$accept_invite", "Spelare {0} har accepterat vänförfrågan.");
        _dictionary.Add("$cancel_invite", "Spelare {0} har avbrutit sin vänförfrågan.");
        //Terminal
        _dictionary.Add("$terminal_set_level", "Du har nått nivå {0}.");
        _dictionary.Add("$terminal_reset_points", "Dina attributpoäng har återställts");

        _dictionary.Add("$strength_tooltip", "<size=20>Styrka kommer att förbättra:</size> \n" +
            "<color=yellow> Öka Fysisk Skada </color> \n" +
            "<color=blue> Öka Bärkapacitet </color> \n" +
            "<color=green> Minska Stamina-förbrukning vid blockering </color> \n" +
            "<color=red> Öka Kritisk Skada vid kritiska träffar </color>");

        _dictionary.Add("$dexterity_tooltip", "<size=20>Smidighet kommer att förbättra:</size> \n" +
            "<color=red> Öka Attackhastighet (ej bågar) </color> \n" +
            "<color=yellow> Minska Stamina-förbrukning vid attack </color> \n" +
            "<color=green> Minska Stamina-förbrukning vid löpning/hopp </color>");

        _dictionary.Add("$intelect_tooltip", "<size=20>Intellekt kommer att förbättra:</size> \n" +
            "<color=green> Öka all Elementalskada </color> \n" +
            "<color=red> Öka basmängden Eitr (när du har Eitr) </color> \n" +
            "<color=red> Öka Eitr-regeneration </color>");

        _dictionary.Add("$endurance_tooltip", "<size=20>Uthållighet kommer att förbättra:</size> \n" +
            "<color=yellow> Öka Stamina-mängden </color> \n" +
            "<color=yellow> Öka Stamina-regeneration </color> \n" +
            "<color=green> Minska Fysisk Skada som tas emot </color>");

        _dictionary.Add("$vigour_tooltip", "<size=20>Kraft kommer att förbättra:</size> \n" +
            "<color=red> Öka HP-mängden </color> \n" +
            "<color=yellow> Hälsoregeneration </color> \n" +
            "<color=green> Minska Elemental Skada som tas emot </color>");

        _dictionary.Add("$special_tooltip", "<size=20>Specialisering kommer att förbättra:</size> \n" +
            "<color=red> Öka Chansen för Kritiska Attacker </color> \n" +
            "<color=blue> Öka Skadan vid Brytning </color> \n" +
            "<color=blue> Öka Hållbarheten hos Byggnadsdelar </color> \n" +
            "<color=green> Öka Skadan vid Trädfällning </color>");

    }
    private void SpanLocalization()
    {
        _dictionary.Add("$attributes", "Atributos");
        _dictionary.Add("$parameter_strength", "Fuerza");
        _dictionary.Add("$parameter_intellect", "Intelecto");
        _dictionary.Add("$free_points", "Puntos disponibles");
        _dictionary.Add("$level", "Nivel");
        _dictionary.Add("$lvl", "Nivel.");
        _dictionary.Add("$exp", "Experiencia");
        _dictionary.Add("$cancel", "Cancelar");
        _dictionary.Add("$apply", "Aceptar");
        _dictionary.Add("$reset_parameters", "Restablecer puntos");
        _dictionary.Add("$no", "No");
        _dictionary.Add("$yes", "Si");
        _dictionary.Add("$get_exp", "Experiencia recibida");
        _dictionary.Add("$reset_point_text", "¿De verdad quieres eliminar todos los puntos por {0} {1}?");
        //Parameter
        _dictionary.Add("$physic_damage", "Daño Físico");
        _dictionary.Add("$add_weight", "Cargar Peso");
        _dictionary.Add("$speed_attack", "Consumo de resistencia de ataque");
        _dictionary.Add("$reduced_stamina", "Consumo de energía (correr, saltar)");
        _dictionary.Add("$magic_damage", "Daño mágico");
        _dictionary.Add("$magic_armor", "Armadura mágica");
        _dictionary.Add("$add_hp", "Aumento de la salud");
        _dictionary.Add("$add_stamina", "Aumento de resistencia");
        _dictionary.Add("$physic_armor", "Armadura física");
        _dictionary.Add("$reduced_stamina_block", "Consumo de energia de bloqueo");
        _dictionary.Add("$regen_hp", "Regeneración de salud");
        _dictionary.Add("$damage", "Daño");
        _dictionary.Add("$armor", "Armadura");
        _dictionary.Add("$survival", "Surpervivencia");

        // new/changed Params 1.7.0
        _dictionary.Add("$parameter_agility", "Destreza");
        _dictionary.Add("$parameter_body", "Resistencia");
        _dictionary.Add("$parameter_vigour", "Vigor");
        _dictionary.Add("$parameter_special", "Especialización");
        _dictionary.Add("$specialother", "Especial");//divheader
        _dictionary.Add("$attack_speed", "Attack speed");
        _dictionary.Add("$attack_stamina", "Velocidad de ataque");
        _dictionary.Add("$crtcDmgMulti", "Multiplicador de Daño Crítico");
        _dictionary.Add("$mining_speed", "Aumento de daño minero");
        _dictionary.Add("$piece_health", "Aumento de la salud de la pieza");
        _dictionary.Add("$tree_cutting", "Aumento del daño del árbol\"");
        _dictionary.Add("$crit_chance", "Aumento de probabilidad crítica");

        _dictionary.Add("$regen_eitr", "regeneración de eitr");
        _dictionary.Add("$stamina_reg", "regeneración de resistencia");
        _dictionary.Add("$add_eitr", "Aumento de EIT");
        //Friends list
        _dictionary.Add("$notify", "<color=#00E6FF>Alerta</color>");
        _dictionary.Add("$friends_list", "Lista de amigos");
        _dictionary.Add("$send", "Enviar");
        _dictionary.Add("$invited", "Invitaciones");
        _dictionary.Add("$friends", "Amigos");
        _dictionary.Add("$online", "Conectado");
        _dictionary.Add("$offline", "Desconectado");
        _dictionary.Add("$not_found", "Player {0} inexistente.");
        _dictionary.Add("$send_invite", "Se ha enviado una solicitud de amistad a {0}.");
        _dictionary.Add("$get_invite", "Has recivido una solicitud de amistad de {0}.");
        _dictionary.Add("$accept_invite", "{0} aceptó la solicitud de amistad.");
        _dictionary.Add("$cancel_invite", "{0} denegó la solicitud de amistad.");
        //Terminal
        _dictionary.Add("$terminal_set_level", "Eres nivel {0}");
        _dictionary.Add("$terminal_reset_points", "Tus puntos de atributos han sido reiniciados");

        _dictionary.Add("$strength_tooltip", "<size=20>La fuerza mejorará:</size> \n" +
        "<color=yellow> Aumento del daño físico </color> \n" +
        "<color=blue> Aumento del peso que puedes llevar </color> \n" +
        "<color=green> Reducción del consumo de resistencia al bloquear </color> \n" +
        "<color=red> Aumento del daño crítico al realizar golpes críticos </color>");

        _dictionary.Add("$dexterity_tooltip", "<size=20>La destreza mejorará:</size> \n" +
        "<color=red> Aumento de la velocidad de ataque (excepto con arcos)</color> \n" +
        "<color=yellow> Reducción del consumo de resistencia al atacar </color> \n" +
        "<color=green> Reducción del consumo de resistencia al correr/saltar </color>");

        _dictionary.Add("$intelect_tooltip", "<size=20>La inteligencia mejorará:</size> \n" +
        "<color=green> Aumento del daño elemental </color>\n" +
        "<color=red> Aumento de la cantidad de Eitr base (una vez que tengas Eitr)</color> \n" +
        "<color=red> Aumento de la regeneración de Eitr</color>");

        _dictionary.Add("$endurance_tooltip", "<size=20>La resistencia mejorará:</size> \n" +
        "<color=yellow> Aumento de la cantidad de resistencia </color>\n" +
        "<color=yellow> Aumento de la regeneración de resistencia </color> \n" +
        "<color=green> Reducción del daño físico recibido </color>");

        _dictionary.Add("$vigour_tooltip", "<size=20>El vigor mejorará:</size> \n" +
        "<color=red> Aumento de la cantidad de puntos de vida </color>\n" +
        "<color=yellow> Regeneración de salud </color> \n" +
        "<color=green> Reducción del daño elemental recibido </color>");

        _dictionary.Add("$special_tooltip", "<size=20>Lo especial mejorará:</size> \n" +
        "<color=red> Aumento de la probabilidad de ataque crítico </color> \n" +
        "<color=blue> Aumento del daño de minería </color> \n" +
        "<color=blue> Aumento de la resistencia de las piezas de construcción </color> \n" +
        "<color=green> Aumento del daño al cortar árboles </color>");
    }

    private void GermanLocalization()
    {

        _dictionary.Add("$attributes", "Eigenschaften");
        _dictionary.Add("$parameter_strength", "Stärke");
        _dictionary.Add("$parameter_intellect", "Intelligenz");
        _dictionary.Add("$free_points", "Verfügbare Punkte");
        _dictionary.Add("$level", "Level");
        _dictionary.Add("$lvl", "Lvl.");
        _dictionary.Add("$exp", "Erfahrung");
        _dictionary.Add("$cancel", "Zurück");
        _dictionary.Add("$apply", "übernehmen");
        _dictionary.Add("$reset_parameters", "Punkte zurücksetzen");
        _dictionary.Add("$no", "Nein");
        _dictionary.Add("$yes", "Ja");
        _dictionary.Add("$get_exp", "Erfahrung erhalten");
        _dictionary.Add("$reset_point_text", "Möchtest du wirklich alle Punkte Löschen? {0} {1}?");

        _dictionary.Add("$physic_damage", "Körperlicher Schaden");
        _dictionary.Add("$add_weight", "Gewicht tragen");
        _dictionary.Add("$speed_attack", "Ausdauerverbrauch angreifen");
        _dictionary.Add("$reduced_stamina", "Ausdauerverbrauch (running, jumping)");
        _dictionary.Add("$magic_damage", "Elementarschaden");
        _dictionary.Add("$magic_armor", " Elementarschaden Rüstung");
        _dictionary.Add("$add_hp", "Gesundheitssteigerung");
        _dictionary.Add("$add_stamina", "Ausdauer erhöt um");
        _dictionary.Add("$physic_armor", "Physische Rüstung");
        _dictionary.Add("$reduced_stamina_block", "Ausdauerverbrauch blockieren");
        _dictionary.Add("$regen_hp", "Heilung");
        _dictionary.Add("$damage", "Schaden");
        _dictionary.Add("$armor", "Rüstung");
        _dictionary.Add("$survival", "überleben");

        // new/changed Params 1.7.0
        _dictionary.Add("$parameter_agility", "Geschicklichkeit");
        _dictionary.Add("$parameter_body", "Ausdauer");
        _dictionary.Add("$parameter_vigour", "Kraft");
        _dictionary.Add("$parameter_special", "Spezialisierung");
        _dictionary.Add("$specialother", "Speziell");//divheader
        _dictionary.Add("$attack_speed", "Angriffsgeschwindigkeit");
        _dictionary.Add("$attack_stamina", "Verbrauch der Angriffsausdauer");
        _dictionary.Add("$crtcDmgMulti", "Multiplikator für kritischen Schaden");
        _dictionary.Add("$mining_speed", "Erhöhter Bergbauschaden");
        _dictionary.Add("$piece_health", "Stückgesundheitserhöhung");
        _dictionary.Add("$tree_cutting", "Baumschaden erhöht");
        _dictionary.Add("$crit_chance", "Erhöhung der kritischen Trefferchance");

        _dictionary.Add("$regen_eitr", "Eitr-Regeneration");
        _dictionary.Add("$stamina_reg", "Regeneration der Ausdauer");
        _dictionary.Add("$add_eitr", "Eitr erhöhen");

        _dictionary.Add("$notify", "<color=#00E6FF>Alarm");
        _dictionary.Add("$friends_list", "Freundesliste");
        _dictionary.Add("$send", "Senden");
        _dictionary.Add("$invited", "Einladungen");
        _dictionary.Add("$friends", "Freunde");
        _dictionary.Add("$online", "Online");
        _dictionary.Add("$offline", "Offline");
        _dictionary.Add("$not_found", "Spieler {0} nicht gefunden.");
        _dictionary.Add("$send_invite", "Eine Freundschaftsanfrage wurde an den Spieler gesendet {0}.");
        _dictionary.Add("$get_invite", "erhielt eine Freundschaftsanfrage von {0}.");
        _dictionary.Add("$accept_invite", "Spieler {0}, hat die Freundschaftsanfrage bestätigt.");
        _dictionary.Add("$cancel_invite", "Spieler {0}, hat die Freundschaftsanfrage abgelehnt.");
        _dictionary.Add("$terminal_set_level", "Du hast {0} level");
        _dictionary.Add("$terminal_reset_points", "Deine Attributspunkte wurden zurückgesetzt");

        _dictionary.Add("$strength_tooltip", "<size=20>Stärke wird verbessern:</size> \n" +
        "<color=yellow> Erhöhten physischen Schaden </color> \n" +
        "<color=blue> Erhöhte Tragfähigkeit </color> \n" +
        "<color=green> Verringerten Block-Stamina-Verbrauch </color> \n" +
        "<color=red> Erhöhten kritischen Schaden bei kritischen Treffern </color>");

        _dictionary.Add("$dexterity_tooltip", "<size=20>Geschicklichkeit wird verbessern:</size> \n" +
        "<color=red> Erhöhte Angriffsgeschwindigkeit (außer Bögen)</color> \n" +
        "<color=yellow> Verringerten Angriffs-Stamina-Verbrauch </color> \n" +
        "<color=green> Verringerten Lauf-/Sprung-Stamina-Verbrauch </color>");

        _dictionary.Add("$intelect_tooltip", "<size=20>Intelligenz wird verbessern:</size> \n" +
        "<color=green> Erhöhten Schaden aller Elemente </color>\n" +
        "<color=red> Erhöhte Grundmenge an Eitr (sobald du Eitr hast)</color> \n" +
        "<color=red> Erhöhte Eitr-Regeneration</color>");

        _dictionary.Add("$endurance_tooltip", "<size=20>Ausdauer wird verbessern:</size> \n" +
        "<color=yellow> Erhöhte Ausdauer </color>\n" +
        "<color=yellow> Erhöhte Ausdauer-Regeneration </color> \n" +
        "<color=green> Verringerten erlittenen physischen Schaden </color>");

        _dictionary.Add("$vigour_tooltip", "<size=20>Vitalität wird verbessern:</size> \n" +
        "<color=red> Erhöhte HP-Menge </color>\n" +
        "<color=yellow> Gesundheitsregeneration </color> \n" +
        "<color=green> Verringerten erlittenen elementaren Schaden </color>");

        _dictionary.Add("$special_tooltip", "<size=20>Besonderheit wird verbessern:</size> \n" +
        "<color=red> Erhöhte Chance für kritische Angriffe </color> \n" +
        "<color=blue> Erhöhten Bergbau-Schaden </color> \n" +
        "<color=blue> Erhöhte Lebenspunkte von Konstruktionsstücken </color> \n" +
        "<color=green> Erhöhten Schaden beim Baumfällen </color>");

    }

    private void ChineseLocalization()
    {
        _dictionary.Add("$attributes", "角色属性");
        _dictionary.Add("$parameter_strength", "力量");
        _dictionary.Add("$parameter_intellect", "智力");
        _dictionary.Add("$free_points", "可分配的属性点");
        _dictionary.Add("$level", "等级");
        _dictionary.Add("$lvl", "等级：");
        _dictionary.Add("$exp", "经验");
        _dictionary.Add("$cancel", "取消");
        _dictionary.Add("$apply", "接受");
        _dictionary.Add("$reset_parameters", "重置属性");
        _dictionary.Add("$no", "否");
        _dictionary.Add("$yes", "是");
        _dictionary.Add("$get_exp", "所需经验");
        _dictionary.Add("$reset_point_text", "你是否要消耗 {0} {1} 重置你的所有属性吗? ");
        _dictionary.Add("$physic_damage", "物理伤害");
        _dictionary.Add("$add_weight", "负重");
        _dictionary.Add("$speed_attack", "攻击耐力值消耗");
        _dictionary.Add("$reduced_stamina", "耐力值消耗（跑步、跳跃）");
        _dictionary.Add("$magic_damage", "元素伤害");
        _dictionary.Add("$magic_armor", "元素抗性");
        _dictionary.Add("$add_hp", "生命值");
        _dictionary.Add("$add_stamina", "耐力值");
        _dictionary.Add("$physic_armor", "物理抗性");
        _dictionary.Add("$reduced_stamina_block", "格挡耐力值消耗");
        _dictionary.Add("$regen_hp", "生命值恢复");
        _dictionary.Add("$damage", "攻击属性");
        _dictionary.Add("$armor", "防御属性");
        _dictionary.Add("$survival", "基础属性");
        _dictionary.Add("$regen_eitr", "埃达之力恢复");
        _dictionary.Add("$stamina_reg", "耐力值恢复");
        _dictionary.Add("$add_eitr", "埃达之力");
        _dictionary.Add("$notify", "<color=#00E6FF>通知</color>");
        _dictionary.Add("$friends_list", "好友列表");
        _dictionary.Add("$send", "发送");
        _dictionary.Add("$invited", "邀请");
        _dictionary.Add("$friends", "好友");
        _dictionary.Add("$online", "在线");
        _dictionary.Add("$offline", "离线");
        _dictionary.Add("$not_found", "未找到 玩家 {0} ！");
        _dictionary.Add("$send_invite", "已向 玩家 {0} 发送好友请求。");
        _dictionary.Add("$get_invite", "收到来自 玩家 {0} 的好友请求。");
        _dictionary.Add("$accept_invite", "玩家 {0} , 已接受好友请求。");
        _dictionary.Add("$cancel_invite", "玩家 {0} , 拒绝了好友请求。");
        _dictionary.Add("$terminal_set_level", "您提升到了 {0} 级！");
        _dictionary.Add("$terminal_reset_points", "您的属性已重置！");

        // new/changed Params 1.7.0
        _dictionary.Add("$parameter_agility", "灵巧");
        _dictionary.Add("$parameter_body", "耐力");
        _dictionary.Add("$parameter_vigour", "活力");
        _dictionary.Add("$parameter_special", "特殊");
        _dictionary.Add("$specialother", "特别的");//divheader
        _dictionary.Add("$attack_speed", "攻击速度");
        _dictionary.Add("$attack_stamina", "攻击体力消耗");
        _dictionary.Add("$crtcDmgMulti", "暴击伤害倍率");
        _dictionary.Add("$mining_speed", "采矿伤害增加");
        _dictionary.Add("$piece_health", "增加生命值");
        _dictionary.Add("$tree_cutting", "树木伤害增加");
        _dictionary.Add("$crit_chance", "暴击率增加");

        _dictionary.Add("$strength_tooltip", "<size=20>力量将增强以下效果：</size> \n" +
        "<color=yellow>增加物理伤害</color> \n" +
        "<color=blue>增加负重</color> \n" +
        "<color=green>减少格挡体力消耗</color> \n" +
        "<color=red>增加爆击伤害</color>");

        _dictionary.Add("$dexterity_tooltip", "<size=20>灵巧将增强以下效果：</size> \n" +
        "<color=red>增加攻击速度（非弓箭）</color> \n" +
        "<color=yellow>减少攻击体力消耗</color> \n" +
        "<color=green>减少奔跑/跳跃体力消耗</color>");

        _dictionary.Add("$intelect_tooltip", "<size=20>智力将增强以下效果：</size> \n" +
        "<color=green>增加所有元素伤害</color> \n" +
        "<color=red>增加基础 Eitr 数量（获得 Eitr 后）</color> \n" +
        "<color=red>增加 Eitr 再生速度</color>");

        _dictionary.Add("$endurance_tooltip", "<size=20>耐力将增强以下效果：</size> \n" +
        "<color=yellow>增加体力量</color> \n" +
        "<color=yellow>增加体力再生速度</color> \n" +
        "<color=green>减少受到的物理伤害</color>");

        _dictionary.Add("$vigour_tooltip", "<size=20>活力将增强以下效果：</size> \n" +
        "<color=red>增加生命值量</color> \n" +
        "<color=yellow>生命值再生</color> \n" +
        "<color=green>减少受到的元素伤害</color>");

        _dictionary.Add("$special_tooltip", "<size=20>特殊将增强以下效果：</size> \n" +
        "<color=red>增加暴击攻击几率</color> \n" +
        "<color=blue>增加采矿伤害</color> \n" +
        "<color=blue>增加建筑物件的耐久度</color> \n" +
        "<color=green>增加伐木伤害</color>");
    }

    private void PortugueseLocalization()
    {
        _dictionary.Add("$attributes", "Atributos");
        _dictionary.Add("$parameter_strength", "Força");
        _dictionary.Add("$parameter_intellect", "Intelecto");
        _dictionary.Add("$free_points", "Pontos disponíveis");
        _dictionary.Add("$level", "Nível");
        _dictionary.Add("$lvl", "Nvl.");
        _dictionary.Add("$exp", "Experiência");
        _dictionary.Add("$cancel", "Cancelar");
        _dictionary.Add("$apply", "Aceitar");
        _dictionary.Add("$reset_parameters", "Redefinir pontos");
        _dictionary.Add("$no", "Não");
        _dictionary.Add("$yes", "Sim");
        _dictionary.Add("$get_exp", "Experiência recebida");
        _dictionary.Add("$reset_point_text", "Você realmente deseja remover todos os pontos para {0} {1}?");
        //Parâmetro
        _dictionary.Add("$physic_damage", "Dano Físico");
        _dictionary.Add("$add_weight", "Peso que pode carregar");
        _dictionary.Add("$reduced_stamina", "Consumo de resistência (correr, pular)");
        _dictionary.Add("$magic_damage", "Dano Elemental");
        _dictionary.Add("$magic_armor", "Redução Elemental");
        _dictionary.Add("$add_hp", "Aumento de Vida");
        _dictionary.Add("$add_stamina", "Aumento de Resistência");
        _dictionary.Add("$physic_armor", "Redução Física");
        _dictionary.Add("$reduced_stamina_block", "Consumo de resistência ao bloquear");
        _dictionary.Add("$regen_hp", "Regeneração de Vida");
        _dictionary.Add("$damage", "Dano");
        _dictionary.Add("$armor", "Armadura");
        _dictionary.Add("$survival", "Sobrevivência");
        _dictionary.Add("$regen_eitr", "Regeneração de Eitr");
        _dictionary.Add("$stamina_reg", "Regeneração de Resistência");
        _dictionary.Add("$add_eitr", "Aumento de Eitr");
        //Novos/alterados Parâmetros 1.7.0
        _dictionary.Add("$parameter_agility", "Destreza");
        _dictionary.Add("$parameter_body", "Resistência");
        _dictionary.Add("$parameter_vigour", "Vigor");
        _dictionary.Add("$parameter_special", "Especialização");
        _dictionary.Add("$specialother", "Especial");//divheader
        _dictionary.Add("$attack_speed", "Velocidade de Ataque");
        _dictionary.Add("$attack_stamina", "Consumo de resistência ao atacar");
        _dictionary.Add("$crtcDmgMulti", "Multiplicador de Dano Crítico");
        _dictionary.Add("$mining_speed", "Aumento de Dano na Mineração");
        _dictionary.Add("$piece_health", "Aumento da Vida das Peças");
        _dictionary.Add("$tree_cutting", "Aumento do Dano ao Cortar Árvores");
        _dictionary.Add("$crit_chance", "Aumento da Chance de Ataque Crítico");

        //Lista de amigos
        _dictionary.Add("$notify", "<color=#00E6FF>Alerta</color>");
        _dictionary.Add("$friends_list", "Lista de Amigos");
        _dictionary.Add("$send", "Enviar");
        _dictionary.Add("$invited", "Convites");
        _dictionary.Add("$friends", "Amigos");
        _dictionary.Add("$online", "Online");
        _dictionary.Add("$offline", "Offline");
        _dictionary.Add("$not_found", "Jogador {0} não encontrado.");
        _dictionary.Add("$send_invite", "Um pedido de amizade foi enviado para o jogador {0}.");
        _dictionary.Add("$get_invite", "Recebeu um pedido de amizade de {0}.");
        _dictionary.Add("$accept_invite", "O jogador {0} aceitou o pedido de amizade.");
        _dictionary.Add("$cancel_invite", "O jogador {0} cancelou o pedido de amizade.");
        //Terminal
        _dictionary.Add("$terminal_set_level", "Você alcançou o nível {0}.");
        _dictionary.Add("$terminal_reset_points", "Os pontos dos seus atributos foram redefinidos.");

        _dictionary.Add("$strength_tooltip", "<size=20>A força aumentará:</size> \n" +
            "<color=yellow> Aumento do Dano Físico </color> \n" +
            "<color=blue> Aumento do Peso que pode carregar </color> \n" +
            "<color=green> Redução do Consumo de Resistência ao Bloquear </color> \n" +
            "<color=red> Aumento do Dano Crítico quando acertar críticos </color>");

        _dictionary.Add("$dexterity_tooltip", "<size=20>A destreza aumentará:</size> \n" +
            "<color=red> Aumento da Velocidade de Ataque (exceto arcos) </color> \n" +
            "<color=yellow> Redução do Consumo de Resistência ao Atacar </color> \n" +
            "<color=green> Redução do Consumo de Resistência ao Correr/Saltar </color>");

        _dictionary.Add("$intelect_tooltip", "<size=20>A inteligência aumentará:</size> \n" +
            "<color=green> Aumento do Dano de Todos os Elementos </color> \n" +
            "<color=red> Aumento da Quantidade de Eitr Base (quando tiver Eitr) </color> \n" +
            "<color=red> Aumento da Regeneração de Eitr </color>");

        _dictionary.Add("$endurance_tooltip", "<size=20>A resistência aumentará:</size> \n" +
            "<color=yellow> Aumento da Quantidade de Resistência </color> \n" +
            "<color=yellow> Aumento da Regeneração de Resistência </color> \n" +
            "<color=green> Redução do Dano Físico Recebido </color>");

        _dictionary.Add("$vigour_tooltip", "<size=20>O vigor aumentará:</size> \n" +
            "<color=red> Aumento da Quantidade de Pontos de Vida </color> \n" +
            "<color=yellow> Regeneração de Vida </color> \n" +
            "<color=green> Redução do Dano Elemental Recebido </color>");

        _dictionary.Add("$special_tooltip", "<size=20>O especializar aumentará:</size> \n" +
            "<color=red> Aumento da Chance de Ataque Crítico </color> \n" +
            "<color=blue> Aumento do Dano de Mineração </color> \n" +
            "<color=blue> Aumento da Vida das Peças de Construção </color> \n" +
            "<color=green> Aumento do Dano ao Cortar Árvores </color>");

    }

    public string this[string key]
    {

        get
        {
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key];
            }
            return "Missing language key";
            //return key;
        }
    }
}