using System;
using System.Collections;
using System.Collections.Generic;

namespace stw
{
    public class links_t
    {
        public links_t next;
        public links_t prev;

        public links_t()
        {
            next = prev = null;
        }
    };
    
    public delegate void call_back(System.Object parm);

    public class tmr_t : links_t
    {
        public uint rotation_count;
        public uint delay;            /* initial delay       */
        public uint periodic_delay;   /* auto-restart if > 0 */
        public uint count;
        public call_back func_ptr;
        public System.Object parm;
    };

    // Ê±¼äÂÖ
    public class timer_wheel
    {
        internal string wheel_name;
        internal uint magic_tag;           /* for sanity check */
        internal uint wheel_size;
        internal uint spoke_index;         /* mod index around wheel */
        internal uint ticks;               /* absolute ticks */
        internal uint granularity;         /* millisecond per tick */

        public uint GetTicks()
        {
            return ticks;
        }

        /*
         * * few book keeping parameters to help engineer the wheel
         */
        internal uint timer_hiwater_mark;
        internal uint timer_active;
        internal uint timer_cancelled;
        internal uint timer_expired;
        internal uint timer_starts;

        internal links_t[] spokes;

        /*
         * range of valid wheel sizes
         */
        const int STW_MIN_WHEEL_SIZE = 32;
        const int STW_MAX_WHEEL_SIZE = 4096;

        /*
         * Granularity of a timer tick in milliseconds   
         */
        const int STW_MIN_GRANULARITY = 1;
        const int STW_MAX_GRANULARITY = 100;

        const uint MAGIC_TAG = 0x0FEDCA3BA;

        public timer_wheel(uint wheel_size_, uint granularity_, string name)
        {
            uint j;
            links_t spoke;

            /*
             * we need to put some bounds to protect against extremely
             * large numbers
             */
            if (wheel_size_ < STW_MIN_WHEEL_SIZE || wheel_size_ > STW_MAX_WHEEL_SIZE)
            {
                wheel_size_ = STW_MAX_WHEEL_SIZE;
            }

            if (granularity_ < STW_MIN_GRANULARITY || granularity_ > STW_MAX_GRANULARITY)
            {
                granularity_ = STW_MIN_GRANULARITY;
            }

            /*
             * allocate memory for wheel spokes
             */
            spokes = new links_t[wheel_size_];
            for (int i = 0; i < wheel_size_; ++i)
                spokes[i] = new links_t();

            /*
             * Initialize the internal tick count at zero, should use
             * safe string lib! 
             */
            wheel_name = name;

            magic_tag = MAGIC_TAG;

            ticks = 0;

            spoke_index = 0;
            granularity = granularity_;
            wheel_size = wheel_size_;

            /*
             * timer stats to tune wheel
             */
            timer_hiwater_mark = 0;
            timer_active = 0;
            timer_cancelled = 0;
            timer_expired = 0;
            timer_starts = 0;

            /*
             * Set all spokes to empty
             */
            for (j = 0; j < wheel_size; j++)
            {
                spoke = spokes[j];
                spoke.next = spoke;    /* empty spoke points to itself */
                spoke.prev = spoke;
            }
        }

        public tmr_t add_timer(uint delay, uint periodic_delay, uint count, call_back user_cb, System.Object parm)
        {
            if (magic_tag != MAGIC_TAG)
                return null;

            tmr_t tmr = new tmr_t();
            
            /*
             * set user call_back and parameter
             */
            tmr.func_ptr = user_cb;
            tmr.parm = parm;
            tmr.delay = delay;
            tmr.periodic_delay = periodic_delay;
            tmr.count = count;
            
            tmr_enqueue(tmr, delay);

            timer_starts++;
            timer_active++;
            if (timer_active > timer_hiwater_mark)
            {
                timer_hiwater_mark = timer_active;
            }

            return tmr;
        }

        public bool cannel(tmr_t tmr)
        {
            links_t next, prev;
           
            if (tmr == null)
                return false;

            if (magic_tag != MAGIC_TAG)
                return false;

            next = tmr.next;
            if (next != null)
            {
                prev = tmr.prev;
                next.prev = prev;
                prev.next = next;
                tmr.next = null;    /* NULL == tmr is free */
                tmr.prev = null;

                /*
                 * stats bookkeeping
                 */
                timer_active--;
                timer_cancelled++;
            }

            return true;
        }

        void tmr_enqueue(tmr_t tmr, uint delay)
        {
            links_t  prev, spoke;

            uint cursor;
            uint ticks;
            uint td;
            
            if (delay < granularity)
            {
                /*
                 * must delay at least one tick, can not delay in past...
                 */
                ticks = 1;
            }
            else
            {
                /*
                 * compute number ticks reqquired to expire the duration
                 */
                ticks = (delay / granularity);
            }
            
            /*
             * tick displacement from current cursor
             */
            td = (ticks % wheel_size);
            
            /*
             * times around the wheel required to expire duration
             */
            tmr.rotation_count = (ticks / wheel_size);
            
            /*
             * calculate cursor to place the timer
             */
            cursor = ((spoke_index + td) % wheel_size);
            
            spoke = spokes[cursor];

            /*
             * We have a timer and now we have a spoke.  All that is left is to
             * link the timer to the spoke's list of timers.  With a doubly linked
             * list, there is no need to check for an empty list.  We simply link
             * it to the end of the list.  This is the same price as putting it
             * on the front of the list but feels more 'right'.
             */
            prev = spoke.prev;
            tmr.next = spoke;      /* append to end of spoke  */
            tmr.prev = prev;

            prev.next = tmr;
            spoke.prev = tmr;
        }

        public void step()
        {
            call_back user_call_back;
            if (magic_tag != MAGIC_TAG)
                return;

            /*
             * keep track of rolling the wheel
             */
            ticks++;
            
            /*
             * advance the index to the next spoke
             */
            spoke_index = (spoke_index + 1) % wheel_size;

            /*
             * Process the spoke, removing timers that have expired.
             * If the timer rotation count is positive
             * decrement and catch the timer on the next wheel revolution.
             */

            tmr_t tmr_r = null;
            links_t spoke, next, prev, tmr;
            spoke = spokes[spoke_index];
            tmr = spoke.next;
            while (tmr != spoke)
            {
                tmr_r = (tmr_t)tmr;
                next = (links_t)tmr.next;
                prev = (links_t)tmr.prev;

                /*
                 * if the timer is a long one and requires one or more rotations
                 * decrement rotation count and leave for next turn.
                 */
                if (tmr_r.rotation_count != 0)
                {
                    tmr_r.rotation_count--;
                }
                else
                {
                    prev.next = next;
                    next.prev = prev;

                    tmr.next = null;
                    tmr.prev = null;

                    /* book keeping */
                    timer_active--;
                    timer_expired++;

                    /*
                     * Invoke the user expiration handler to do the actual work.
                     */
                    user_call_back = tmr_r.func_ptr;
                    user_call_back(tmr_r.parm);

                    tmr_r.count--;

                    /*
                     * automatically restart the timer if periodic_delay > 0
                     */
                    if (tmr_r.count > 0 && tmr_r.periodic_delay > 0)
                    {
                        tmr_enqueue(tmr_r, tmr_r.periodic_delay);
                        timer_active++;
                    }
                }

                tmr = next;
            }
        }
    }
}