using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battle_wolf : StateMachineBehaviour
{
    wolf_setting wolf;
    Story1 story1;
    player_setting witch;

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        story1 = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<Story1>();
        witch = GameObject.FindGameObjectWithTag("player").GetComponent<player_setting>();

        string tag_enemy = "wolf" + story1.question_last;
        wolf = GameObject.FindGameObjectWithTag(tag_enemy).GetComponent<wolf_setting>();
        
        bool question_true = story1.get_true_answer();

        int to_waypoint_enemy = story1.question_last + story1.question_last;
        int to_waypoint_next_question = to_waypoint_enemy + 1;

        story1.close_last_question();
        story1.change_battle();

        if(question_true == true)
        {
            witch.walk();
            witch.custom_waypoint(to_waypoint_enemy);
            witch.move_player_to_enemy();

            bool player_attack = witch.get_attack();

            if(player_attack == true)
            {
                GameObject.FindGameObjectWithTag(tag_enemy).active = false;
                witch.walk();
                witch.custom_waypoint(to_waypoint_next_question);
                witch.reset_attack();
                story1.to_next_question();
                story1.reset_true_answer();
                story1.open_next_question();
            }
        }else{
            wolf.walk();
            wolf.waypoint_attack();
            wolf.move_attack();

            bool finish_battle = wolf.get_finish();
            bool after_attack = wolf.get_attack();

            if(finish_battle == true && after_attack == true)
            {
                wolf.flip_rotation_X();
                wolf.walk();
                wolf.waypoint_offset();
                wolf.move_offside();

                bool destroy_battle = wolf.get_destroy();
                if(destroy_battle == true)
                {
                    wolf.reset_attack();
                    witch.walk();
                    witch.custom_waypoint(to_waypoint_next_question);
                    witch.reset_attack();
                    story1.to_next_question();
                    story1.open_next_question();
                    GameObject.FindGameObjectWithTag(tag_enemy).active = false;
                }
            }
        }


    }

}